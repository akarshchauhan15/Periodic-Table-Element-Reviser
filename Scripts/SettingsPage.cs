using Godot;
using System.ComponentModel;

public partial class SettingsPage : Control
{
    public static bool IsSoundEnabled = true;
    public static float AnimationFadeDuration = 0.2f;

    OptionButton ThemeOption;
    Button SoundButton;
    Theme[] Themes;

    string[][] ControlLocations = [
    ["HomePage/StartButton"],
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
        ["ResultPage/GreatPanel"],
        ["SettingsPage/Settings/SoundButton", "ShadowedButton"],
        ["SettingsPage/ExitButton", "ShadowedButton"],
        ["SettingsPage/Settings/ThemeOption"],
    ];
    public override void _Ready()
    {
        Themes = [GD.Load<Theme>("res://Themes/Modern.theme"), GD.Load<Theme>("res://Themes/Metro.theme")];

        ThemeOption = GetNode<OptionButton>("Settings/ThemeOption");
        SoundButton = GetNode<Button>("Settings/SoundButton");

        ThemeOption.ItemSelected += SetTheme;
        SoundButton.Toggled += SoundButtonToggled;
        GetNode<Button>("ExitButton").Pressed += OnExitPressed;

        SetSettings();
    }
    private void SetSettings()
    {
        ThemeOption.Select((int)ConfigController.Config.GetValue("Settings", "Theme", 0));
        SetTheme((long)ConfigController.Config.GetValue("Settings", "Theme", 0));
        SoundButton.ButtonPressed = (bool)ConfigController.Config.GetValue("Settings", "Sound", true);
        SoundButtonToggled((bool)ConfigController.Config.GetValue("Settings", "Sound", true));

        GetNode<Label>("Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();
    }
    private void SetTheme(long Index)
    {
        foreach (string[] Location in ControlLocations)
        {
            Control control = GetParent().GetNodeOrNull<Control>(Location[0]);

            if (control == null)
                return;

            control.Theme = Themes[Index];

            if (Location.Length == 2)
                control.ThemeTypeVariation = Location[1];
        }
        ConfigController.SaveSettings("Settings", "Theme", Index);
    }
    private void SoundButtonToggled(bool SoundEnabled)
    {
        if (SoundEnabled) { 
            SoundButton.Text = "Enabled";}
        else
            SoundButton.Text = "Disabled";

        IsSoundEnabled = SoundEnabled;
        ConfigController.SaveSettings("Settings", "Sound", SoundEnabled);
    }
    private void OnExitPressed() => GetParent<Hud>().AnimatePages(this, GetNode<HomePage>("../HomePage"));
}