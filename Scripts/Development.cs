using Godot;
using System;

public partial class Development : Control
{
    public override void _Ready()
    {
        GetNode<Label>("Version").Text = ProjectSettings.GetSetting("application/config/version").ToString();
    }
}
