using Godot;

public partial class HomePage : Control
{
    Label UpdateLabel;
    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += ProceedToSelection;
        GetNode<Button>("PeriodicTableButton").Pressed += OpenPeriodicTablePage;
        GetNode<TextureButton>("SettingsButton").Pressed += OpenSettings;

        UpdateLabel = GetNode<Label>("UpdateLabel");
    }
    public void PopupUpdatePrompt(bool UpdateAvailiable)
    {
        if (!UpdateAvailiable)
            UpdateLabel.QueueFree();

        Tween tween = CreateTween();
        tween.TweenProperty(UpdateLabel, "position", new Vector2(-100, 0), 0.3f).AsRelative().SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
    }
    private void ProceedToSelection()
    {
        GetNode<SelectionPage>("../SelectionPage").LoadValues();
        GetParent<Hud>().ContinuePage(this);
    }
    private void OpenSettings() => GetParent<Hud>().AnimatePages(this, GetNode<SettingsPage>("../SettingsPage"));
    private void OpenPeriodicTablePage() => GetParent<Hud>().AnimatePages(this, GetNode<PeriodicTablePage>("../PeriodicTablePage"));
    public void EnableSound() => AudioServer.SetBusMute(0, false);
}
