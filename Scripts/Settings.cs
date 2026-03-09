using Godot;
using Godot.Collections;

public partial class Settings : Control
{
    public static float AnimationFadeDuration = 0.2f;
    public static float VibrationFeedbackIntensity = 0f;

    [Signal]
    public delegate void ThemeChangedEventHandler();

    Updates UpdatesPart;
    AudioStreamPlayer UISound;
    HttpRequest VersionRequest;

    OptionButton ThemeOption;
    OptionButton BackgroundOption;
    ToggleButton SoundButton;
    ToggleButton FeedbackButton;
    ToggleButton ImmersiveButton;
    ToggleButton UpdateButton;
    ToggleButton PromptButton;
    Timer UpdateCheckTimer;
    
    Theme[] Themes;
    
    string[][] ControlLocations = [
        ["HomePage/StartButton"],
        ["HomePage/PeriodicTableButton"],
        ["HomePage/UpdateLabel/Panel"],
        ["SelectionPage/ContinueButton", "HighlightedButton"],
        ["SelectionPage/BackButton", "ShadowedButton"],
        ["SelectionPage/GivenOption"],
        ["SelectionPage/ReturnOption"],
        ["CollectionPage/ContinueButton", "HighlightedButton"],
        ["CollectionPage/BackButton", "ShadowedButton"],
        ["CollectionPage/Type"],
        ["CollectionPage/Collection"],
        ["ConfirmationPage/ContinueButton", "HighlightedButton"],
        ["ConfirmationPage/BackButton", "ShadowedButton"],
        ["ActionPage/ExitButton", "ShadowedButton"],
        ["ActionPage/LineEdit"],
        ["ActionPage/ExitTip"],
        ["ResultPage/ContinueButton", "HighlightedButton"],
        ["ResultPage/RetryButton", "ShadowedButton"],
        ["ResultPage/ToggleButton"],
        ["ResultPage/WrongElements/GreatPanel"],
        ["SettingsPage/ExitButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/SoundButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/FeedbackButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/ImmersiveButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/UpdateButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/PromptButton", "ShadowedButtoton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/ThemeOption"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/BackgroundOption"],
        ["PeriodicTablePage/BackButton", "ShadowedButton"],
        ["PeriodicTablePage/Selected", "PeriodicLabelPanel"],
        ["PeriodicTablePage/Selected/Name/Panel", "ElementNameHolder"],
        ["PeriodicTablePage/ScaleController", "HolderPanel"],
        ["PeriodicTablePage/ScaleController/IncreaseScale", "ShadowedButton"],
        ["PeriodicTablePage/ScaleController/DecreaseScale", "ShadowedButton"],
        ["PeriodicTablePage/ScaleController/ResetScale", "ShadowedButton"],
    ];

    public static Color[] BackgroundColours = [
        new Color("0b1531"), // Default
        new Color("0e2d39"), // Sea Green
        new Color("23134b"), // Galaxy
        new Color("28251b"), // Wood
        new Color("242429"), // Grey
        Colors.Black
        ];

    public override void _Ready()
    {
        Themes = [GD.Load<Theme>("res://Themes/Modern.theme"), GD.Load<Theme>("res://Themes/Metro.theme")];

        ThemeOption = GetNode<OptionButton>("ThemeOption");
        BackgroundOption = GetNode<OptionButton>("BackgroundOption");
        SoundButton = GetNode<ToggleButton>("SoundButton");
        FeedbackButton = GetNode<ToggleButton>("FeedbackButton");
        ImmersiveButton = GetNode<ToggleButton>("ImmersiveButton");
        UpdateButton = GetNode<ToggleButton>("UpdateButton");
        PromptButton = GetNode<ToggleButton>("PromptButton");
        UpdateCheckTimer = GetNode<Timer>("UpdateCheckTimer");

        UISound = GetTree().Root.GetNode<AudioStreamPlayer>("Main/HUD/Audio/UI");
        UpdatesPart = GetNode<Updates>("../Updates");

        VersionRequest = new HttpRequest();
        AddChild(VersionRequest);
        VersionRequest.RequestCompleted += UpdateRequestCompleted;

        ThemeOption.ItemSelected += (long Index) => { SetTheme(Index); Hud.SendSoundAndFeedback(); } ;
        BackgroundOption.ItemSelected += SetBackground;
        SoundButton.Toggled += SoundButtonToggled;
        FeedbackButton.Toggled += HapticFeedbackButtonToggled;
        ImmersiveButton.Toggled += ImmersiveModeButtonToggled;
        UpdateButton.Toggled += UpdateButtonToggled;
        PromptButton.Toggled += PromptButtonToggled;
        UpdateCheckTimer.Timeout += CheckForUpdate;
        //UpdateCheckTimer.Timeout += () => ((HomePage)Hud.Pages[0]).PopupUpdatePrompt(true); //For testing purposes.

        SetSettings();
    }
    private void SetSettings()
    {
        AudioServer.SetBusMute(0, true);   

        BackgroundOption.Select((int)ConfigController.Config.GetValue("Settings", "Background", 0));

        bool SoundEnabled = (bool)ConfigController.Config.GetValue("Settings", "Sound", true);
        SoundButton.InitialValueSet(SoundEnabled);
        SoundButtonToggled(SoundEnabled);
        AudioServer.SetBusMute(1, !SoundEnabled);

        bool CheckForUpdatesEnabled = (bool)ConfigController.Config.GetValue("Settings", "CheckForUpdates", true);
        UpdateButton.InitialValueSet(CheckForUpdatesEnabled);
        if (!CheckForUpdatesEnabled) UpdateCheckTimer.QueueFree();

        bool PromptEnabled = (bool)ConfigController.Config.GetValue("Settings", "UpdatePrompt", true);
        PromptButton.InitialValueSet(PromptEnabled);
        GetTree().Root.GetNode<Label>("Main/HUD/HomePage/UpdateLabel").Visible = PromptEnabled;

        bool FeedbackEnabled = (bool)ConfigController.Config.GetValue("Settings", "Feedback", true);
        FeedbackButton.InitialValueSet(FeedbackEnabled);
        HapticFeedbackButtonToggled(FeedbackEnabled);

        bool ImmersiveModeEnabled = (bool)ConfigController.Config.GetValue("Settings", "ImmersiveMode", true);
        ImmersiveButton.InitialValueSet(ImmersiveModeEnabled);
        DisplayServer.WindowSetMode(ImmersiveModeEnabled ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

        if (OS.GetName() == "Android") 
        {
            Vector2 DisplayArea = ImmersiveModeEnabled ? DisplayServer.WindowGetSize() : DisplayServer.WindowGetSize(); //- new Vector2(0, 30);
            GetTree().Root.GetNode<Hud>("Main/HUD").Size = DisplayArea;
        }

        ThemeOption.Select((int)ConfigController.Config.GetValue("Settings", "Theme", 0));
        SetTheme((long)ConfigController.Config.GetValue("Settings", "Theme", 0));
    }
    private void SetTheme(long Index)
    {
        foreach (string[] Location in ControlLocations)
        {
            Control control = GetTree().Root.GetNode<Hud>("Main/HUD").GetNode<Control>(Location[0]);

            control.Theme = Themes[Index];
            
            if (Location.Length == 2)
                control.ThemeTypeVariation = Location[1];
        }
        ConfigController.SaveSettings("Settings", "Theme", Index);

        EmitSignal(SignalName.ThemeChanged);
    }
    private void SetBackground(long Index)
    {
        GetTree().Root.GetNode<ColorRect>("Main/Background").Color = BackgroundColours[Index];
        ConfigController.SaveSettings("Settings", "Background", Index);
        Hud.SendSoundAndFeedback();
    }
    private void SoundButtonToggled(bool SoundEnabled)
    {
        ConfigController.SaveSettings("Settings", "Sound", SoundEnabled);

        AudioServer.SetBusMute(1, !SoundEnabled);
    }
    private void UpdateButtonToggled(bool CheckForUpdatesEnabled)
    {
        ConfigController.SaveSettings("Settings", "CheckForUpdates", CheckForUpdatesEnabled);
    }
    private void PromptButtonToggled(bool PromptEnabled)
    {
        ((HomePage)Hud.Pages[0]).UpdateLabel.Visible = PromptEnabled;
        ConfigController.SaveSettings("Settings", "UpdatePrompt", PromptEnabled);
    }
    private void HapticFeedbackButtonToggled(bool FeedbackEnabled)
    {   
        VibrationFeedbackIntensity = (FeedbackEnabled) ? 0.15f : 0;
        ConfigController.SaveSettings("Settings", "Feedback", FeedbackEnabled);
    }
    private void ImmersiveModeButtonToggled(bool ImmersiveModeEnabled)
    {       
        DisplayServer.WindowSetMode(ImmersiveModeEnabled ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

        Vector2 DisplayArea = ImmersiveModeEnabled ?  DisplayServer.WindowGetSize() : DisplayServer.GetDisplaySafeArea().Size - new Vector2(0, 30);

        if (OS.GetName() == "Android")
        { 
            Tween tween = CreateTween();
            tween.TweenProperty(GetTree().Root.GetNode<Hud>("Main/HUD"), "size", DisplayArea, 0.2).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        }
        ConfigController.SaveSettings("Settings", "ImmersiveMode", ImmersiveModeEnabled);
    }
    public void CheckForUpdate()
    {
        string url = "https://api.github.com/repos/akarshchauhan15/Periodic-Table-Element-Reviser/releases/latest";

        string[] Headers = ["User-Agent: MyGodotApp"];

        Error Err = VersionRequest.Request(url, Headers);
        if (Err != Error.Ok) UpdatesPart.UpdateCode(2);

        Input.VibrateHandheld(20, VibrationFeedbackIntensity);
    }
    private void UpdateRequestCompleted(long result, long response, string[] headers, byte[] body)
    {
        if (response != 200) 
        {
            int code = 2;
            if (response != 0) code++;
            UpdatesPart.UpdateCode(code);
            return;
        }

        Json json = new Json();
        Error ParseError = json.Parse(body.GetStringFromUtf8());

        if (ParseError != Error.Ok) { UpdatesPart.UpdateCode(3); return; }

        Dictionary FetchedData = (Dictionary)json.Data;
        string LatestVersion = FetchedData["tag_name"].ToString();

        bool UpdateAvailiable = (LatestVersion != ProjectSettings.GetSetting("application/config/version").ToString());
        int Code = (UpdateAvailiable) ? 0 : 1;
        ((HomePage)Hud.Pages[0]).PopupUpdatePrompt(UpdateAvailiable);

        UpdatesPart.UpdateCode(Code);
    }
}