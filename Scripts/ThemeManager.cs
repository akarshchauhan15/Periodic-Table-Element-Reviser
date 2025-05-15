using Godot;

public partial class ThemeManager : Node
{
    Theme Metro;
    Theme Modern;

    Theme[] Themes;
    public Theme CurrentTheme;
    Hud Hud;

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
        ["ResultPage/ContinueButton", "HighlightedButton"],
        ["ResultPage/RetryButton", "ShadowedButton"],
        ["SettingsPage/SoundButton", "ShadowedButton"],
        ["SettingsPage/ExitButton", "ShadowedButton"],
        ["SettingsPage/ThemeOption"],
    ];

    public override void _Ready()
    {
        //Metro = (ResourceLoader.Load<Resource>("res://Themes/Metro.theme")).
        //Modern = (ResourceLoader.Load<PackedScene>("res://Themes/Modern.theme")).Instantiate<Theme>();
        Metro = GD.Load<Theme>("res://Themes/Metro.theme");
        Modern = GD.Load<Theme>("res://Themes/Modern.theme");
        Themes = [Modern, Metro];

        Hud = GetTree().Root.GetNode<Hud>("Main/HUD");
        SetTheme((int)ConfigController.Config.GetValue("Settings", "Theme", 0));
    }
    public void SetTheme(int Index)
    { 
        CurrentTheme = Themes[Index];

        foreach (string[] Location in ControlLocations)
        {
            Control control = Hud.GetNode<Control>(Location[0]);
            control.Theme = CurrentTheme;

            if (Location.Length == 2)
                control.ThemeTypeVariation = Location[1];
        }
        ConfigController.SaveSettings("Settings", "Theme", Index);
    }
}