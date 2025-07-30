using Godot;
using Godot.Collections;

public partial class Settings : Control
{
    //public static bool IsSoundEnabled = true;
    public static float AnimationFadeDuration = 0.2f;

    AudioStreamPlayer UISound;

    HttpRequest VersionRequest;

    OptionButton ThemeOption;
    OptionButton BackgroundOption;
    ToggleButton SoundButton;
    ToggleButton UpdateButton;
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
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/UpdateButton", "ShadowedButton"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/ThemeOption"],
        ["SettingsPage/SwipeContainer/HBoxContainer/Settings/BackgroundOption"],
        ["PeriodicTablePage/BackButton", "ShadowedButton"],
        ["PeriodicTablePage/ScaleController", "HolderPanel"],
        ["PeriodicTablePage/ScaleController/IncreaseScale", "ShadowedButton"],
        ["PeriodicTablePage/ScaleController/DecreaseScale", "ShadowedButton"],
        ["PeriodicTablePage/ScaleController/ResetScale", "ShadowedButton"],
    ];

    public static Color[] BackgroundColours = [
        new Color("0b1531"),
        new Color("242429"),
        Colors.Black
        ];

    public override void _Ready()
    {
        Themes = [GD.Load<Theme>("res://Themes/Modern.theme"), GD.Load<Theme>("res://Themes/Metro.theme")];

        ThemeOption = GetNode<OptionButton>("ThemeOption");
        BackgroundOption = GetNode<OptionButton>("BackgroundOption");
        SoundButton = GetNode<ToggleButton>("SoundButton");
        UpdateButton = GetNode<ToggleButton>("UpdateButton");
        UpdateCheckTimer = GetNode<Timer>("UpdateCheckTimer");

        UISound = GetTree().Root.GetNode<AudioStreamPlayer>("Main/HUD/Audio/UI");

        VersionRequest = new HttpRequest();
        AddChild(VersionRequest);
        VersionRequest.RequestCompleted += UpdateRequestCompleted;

        ThemeOption.ItemSelected += SetTheme;
        BackgroundOption.ItemSelected += SetBackground;
        SoundButton.Toggled += SoundButtonToggled;
        UpdateButton.Toggled += UpdateButtonToggled;
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

        ThemeOption.Select((int)ConfigController.Config.GetValue("Settings", "Theme", 0));
        SetTheme((long)ConfigController.Config.GetValue("Settings", "Theme", 0));
    }
    private void SetTheme(long Index)
    {
        foreach (string[] Location in ControlLocations)
        {
            Control control = GetTree().Root.GetNode<Hud>("Main/HUD").GetNode<Control>(Location[0]);

            if (control == null)
                continue;

            control.Theme = Themes[Index];

            if (Location.Length == 2)
                control.ThemeTypeVariation = Location[1];
        }
        ConfigController.SaveSettings("Settings", "Theme", Index);
        UISound.Play();
    }
    private void SetBackground(long Index)
    {
        GetTree().Root.GetNode<ColorRect>("Main/Background").Color = BackgroundColours[Index];
        ConfigController.SaveSettings("Settings", "Background", Index);
        UISound.Play();
    }
    private void SoundButtonToggled(bool SoundEnabled)
    {
        ConfigController.SaveSettings("Settings", "Sound", SoundEnabled);

        AudioServer.SetBusMute(1, !SoundEnabled);
        UISound.Play();
    }
    private void UpdateButtonToggled(bool CheckForUpdatesEnabled)
    {
        ConfigController.SaveSettings("Settings", "CheckForUpdates", CheckForUpdatesEnabled);
        UISound.Play();
    }
    private void CheckForUpdate()
    {
        string url = "https://api.github.com/repos/akarshchauhan15/Periodic-Table-Element-Reviser/releases/latest";

        string[] Headers = ["User-Agent: MyGodotApp"];

        Error Err = VersionRequest.Request(url, Headers);
        if (Err != Error.Ok) GD.PrintErr("Failed to send request: " + Err);
    }
    private void UpdateRequestCompleted(long result, long response, string[] headers, byte[] body)
    {
        if (response != 200) { GD.PrintErr("GitHUb API error: ", response); return; }

        Json json = new Json();
        Error ParseError = json.Parse(body.GetStringFromUtf8());

        if (ParseError != Error.Ok) { GD.PrintErr("Failed to parse JSON: ", ParseError); return; }

        Dictionary FetchedData = (Dictionary)json.Data;
        string LatestVersion = FetchedData["tag_name"].ToString();

        GetNode<HomePage>("../../HomePage").PopupUpdatePrompt(LatestVersion != ProjectSettings.GetSetting("application/config/version").ToString());
    }
}