using Godot;

public partial class HomePage : Control
{
    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += ProceedToSelection;
        GetNode<TextureButton>("SettingsButton").Pressed += OpenSettings;
    }
    private void ProceedToSelection()
    {
        GetNode<SelectionPage>("../SelectionPage").LoadValues();
        GetParent<Hud>().ContinuePage(this);
    }
    private void OpenSettings() => GetParent<Hud>().AnimatePages(this, GetNode<SettingsPage>("../SettingsPage"));
    public void EnableSound() => AudioServer.SetBusMute(0, false);
}
