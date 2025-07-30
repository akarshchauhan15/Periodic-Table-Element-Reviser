using Godot;

public partial class ToggleButton : Button
{
    Tween tween;
    StyleBoxFlat Background;
    StyleBoxFlat Switch;

    Panel BackgroundPanel;
    Panel SwitchPanel;
    public override void _Ready()
    {
        base._Ready();

        Toggled += OnToggled;
        ThemeChanged += SetTheme;

        BackgroundPanel = GetNode<Panel>("Background");
        SwitchPanel = GetNode<Panel>("Background/Control/Switch");

        Background = GetThemeStylebox("panel", "ToggleButtonBackground").Duplicate() as StyleBoxFlat;
        BackgroundPanel.AddThemeStyleboxOverride("panel", Background);

        Switch = GetThemeStylebox("panel", "ToggleButtonSwitch").Duplicate() as StyleBoxFlat;
        SwitchPanel.AddThemeStyleboxOverride("panel", Switch);
    }
    public void InitialValueSet(bool Value)
    {
        SetPressedNoSignal(Value);

        string Color = Value ? "On" : "Off";

        Background.BgColor = GetThemeColor(Color + "Color", "ToggleButtonBackground");
        Switch.BgColor = GetThemeColor(Color + "Color", "ToggleButtonSwitch");
        if (Value) SwitchPanel.Position += new Vector2(GetThemeConstant("Displacement", "ToggleButtonSwitch"), 0);
    }
    public void SetTheme()
    {
        Background = GetThemeStylebox("panel", "ToggleButtonBackground").Duplicate() as StyleBoxFlat;
        BackgroundPanel.AddThemeStyleboxOverride("panel", Background);

        Switch = GetThemeStylebox("panel", "ToggleButtonSwitch").Duplicate() as StyleBoxFlat;
        SwitchPanel.AddThemeStyleboxOverride("panel", Switch);

        SwitchPanel.Position = new Vector2(GetThemeConstant("Default", "ToggleButtonSwitch"), SwitchPanel.Position.Y);
        SwitchPanel.Size = new Vector2(GetThemeConstant("Size", "ToggleButtonSwitch"), SwitchPanel.Size.Y);
        SwitchPanel.GetChild<Label>(0).AddThemeColorOverride("font_color", GetThemeColor("TextColor", "ToggleButtonBackground"));

        string Color = ButtonPressed ? "On" : "Off";

        Background.BgColor = GetThemeColor(Color + "Color", "ToggleButtonBackground");
        Switch.BgColor = GetThemeColor(Color + "Color", "ToggleButtonSwitch");
        if (ButtonPressed) SwitchPanel.Position += new Vector2(GetThemeConstant("Displacement", "ToggleButtonSwitch"), 0);
    }
    private void OnToggled(bool Pressed)
    {
        if (tween != null) tween.Kill();

        tween = CreateTween();
        tween.SetParallel(true);

        string Color = Pressed ? "On" : "Off";
        int Direction = Pressed ? 1 : -1;

        tween.TweenCallback(Callable.From(() => MouseFilter = MouseFilterEnum.Ignore));
        tween.TweenProperty(Background, "bg_color", GetThemeColor(Color + "Color", "ToggleButtonBackground"), 0.25f);
        tween.TweenProperty(Switch, "bg_color", GetThemeColor(Color + "Color", "ToggleButtonSwitch"), 0.25f);
        tween.TweenProperty(SwitchPanel, "position", new Vector2(Direction * SwitchPanel.GetThemeConstant("Displacement"), 0), 0.25f).AsRelative().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quad);
        tween.TweenCallback(Callable.From(() => MouseFilter = MouseFilterEnum.Stop)).SetDelay(0.3f);
    }
}