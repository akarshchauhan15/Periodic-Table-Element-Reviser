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
        Hud.ContinuePage(this);
    }
    private void OpenSettings()
    {
        Hide();
        GetNode<Control>("../SettingsPage").Show();
    }
}
