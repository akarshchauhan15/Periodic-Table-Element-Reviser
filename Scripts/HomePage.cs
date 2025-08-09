using Godot;

public partial class HomePage : Control
{
    public Label UpdateLabel;
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
        {
            UpdateLabel.Position = new Vector2(1089, 1834);
            return;
        }

        Tween tween = CreateTween();
        tween.TweenProperty(UpdateLabel, "position", new Vector2(989, 1834), 0.3f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
    }
    private void ProceedToSelection()
    {
        GetNode<SelectionPage>("../SelectionPage").LoadValues();
        GetParent<Hud>().ContinuePage(this);
    }
    private void OpenSettings() { GetParent<Hud>().AnimatePages(this, GetNode<Control>("../SettingsPage"));  GetNode<SettingsPage>("../SettingsPage").ResetPosition(); }
    private void OpenPeriodicTablePage() => GetParent<Hud>().AnimatePages(this, GetNode<PeriodicTablePage>("../PeriodicTablePage"));
    public void EnableSound() => AudioServer.SetBusMute(0, false);
}
