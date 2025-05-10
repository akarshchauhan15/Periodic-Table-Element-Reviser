using Godot;
using System;

public partial class HomePage : Control
{
    public override void _Ready()
    {
        GetNode<Button>("StartButton").Pressed += ProceedToSelection;
    }
    private void ProceedToSelection()
    {
        GetNode<SeletionPage>("../SelectionPage").LoadValues();
        Hud.ContinuePage(this);
    }
}
