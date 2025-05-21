using Godot;
using Godot.Collections;

public partial class ScreenLoader : Control
{
    TextureRect ProgressRect;
    string Path = "res://Scenes/hud.tscn";
    float DefaultProgressRectX;

    Array Progress = new Array();
    public override void _Ready()
    {
        ProgressRect = GetNode<TextureRect>("ProgressRect");
        DefaultProgressRectX = ProgressRect.Size.X;
        ResourceLoader.LoadThreadedRequest(Path, "", true);
    }
    public override void _PhysicsProcess(double delta)
    {
        ResourceLoader.LoadThreadedGetStatus(Path, Progress);

        ProgressRect.Size = new Vector2((float)Progress[0] * DefaultProgressRectX, ProgressRect.Size.Y);

        if ((double)Progress[0] >= 1)
        {
            PackedScene HudScene = (PackedScene) ResourceLoader.LoadThreadedGet(Path);
            GetParent().AddChild(HudScene.Instantiate<Hud>());
            QueueFree();
        }
        
    }
}
