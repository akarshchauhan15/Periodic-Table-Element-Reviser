using Godot;
using Godot.Collections;

public partial class SettingsPage : Control
{
    public static bool IsSoundEnabled = true;
    public static float AnimationFadeDuration = 0.2f;

    HttpRequest VersionRequest;

    OptionButton ThemeOption;
    OptionButton BackgroundOption;
    ToggleButton SoundButton;
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
        ["SettingsPage/Settings/SoundButton", "ShadowedButton"],
        ["SettingsPage/ExitButton", "ShadowedButton"],
        ["SettingsPage/Settings/ThemeOption"],
        ["SettingsPage/Settings/BackgroundOption"],
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

        ThemeOption = GetNode<OptionButton>("Settings/ThemeOption");
        BackgroundOption = GetNode<OptionButton>("Settings/BackgroundOption");
        SoundButton = GetNode<ToggleButton>("Settings/SoundButton");
        UpdateCheckTimer = GetNode<Timer>("UpdateCheckTimer");

        VersionRequest = new HttpRequest();
        AddChild(VersionRequest);
        VersionRequest.RequestCompleted += UpdateRequestCompleted;

        ThemeOption.ItemSelected += SetTheme;
        BackgroundOption.ItemSelected += SetBackground;
        SoundButton.Toggled += SoundButtonToggled;
        //UpdateCheckTimer.Timeout += CheckForUpdate;  
        //UpdateCheckTimer.Timeout += () => GetNode<HomePage>("../HomePage").PopupUpdatePrompt(true); //For testing purposes.
        GetNode<Button>("ExitButton").Pressed += OnExitPressed;

        SetSettings();
    }
    private void SetSettings()
    {
        AudioServer.SetBusMute(0, true);   

        BackgroundOption.Select((int)ConfigController.Config.GetValue("Settings", "Background", 0));

        SoundButton.InitialValueSet((bool)ConfigController.Config.GetValue("Settings", "Sound", true));
        SoundButtonToggled((bool)ConfigController.Config.GetValue("Settings", "Sound", true));
        AudioServer.SetBusMute(1, !(bool)ConfigController.Config.GetValue("Settings", "Sound", true));

        ThemeOption.Select((int)ConfigController.Config.GetValue("Settings", "Theme", 0));
        SetTheme((long)ConfigController.Config.GetValue("Settings", "Theme", 0));

        GetNode<Label>("Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();
    }
    private void SetTheme(long Index)
    {
        foreach (string[] Location in ControlLocations)
        {
            Control control = GetParent().GetNodeOrNull<Control>(Location[0]);

            if (control == null)
                continue;

            control.Theme = Themes[Index];

            if (Location.Length == 2)
                control.ThemeTypeVariation = Location[1];
        }
        ConfigController.SaveSettings("Settings", "Theme", Index);
        GetNode<AudioStreamPlayer>("../Audio/UI").Play();
    }
    private void SetBackground(long Index)
    {
        GetTree().Root.GetNode<ColorRect>("Main/Background").Color = BackgroundColours[Index];
        ConfigController.SaveSettings("Settings", "Background", Index);
        GetNode<AudioStreamPlayer>("../Audio/UI").Play();
    }
    private void SoundButtonToggled(bool SoundEnabled)
    {
        IsSoundEnabled = SoundEnabled;
        ConfigController.SaveSettings("Settings", "Sound", SoundEnabled);

        AudioServer.SetBusMute(1, !SoundEnabled);
        GetNode<AudioStreamPlayer>("../Audio/UI").Play();
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

        GetNode<HomePage>("../HomePage").PopupUpdatePrompt(LatestVersion != ProjectSettings.GetSetting("application/config/version").ToString());
    }
    private void OnExitPressed() => GetParent<Hud>().AnimatePages(this, GetNode<HomePage>("../HomePage"));
}