using Godot;

public partial class HomePage : Control
{
    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += ProceedToSelection;
    }
    private void ProceedToSelection()
    {
        GetNode<SelectionPage>("../SelectionPage").LoadValues();
        Hud.ContinuePage(this);
    }
}
