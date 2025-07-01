using Godot;
using Godot.Collections;

public partial class ScreenLoader : Control
{
    TextureRect ProgressRect;
    string Path = "res://Scenes/hud.tscn";
    float DefaultProgressRectX;

    Array Progress = new();
    public override void _Ready()
    {
        ProgressRect = GetNode<TextureRect>("ProgressRect");

        DefaultProgressRectX = ProgressRect.Size.X;
        ProgressRect.Size = new Vector2(0, ProgressRect.Size.Y);

        GetNode<ColorRect>("../Background").Color = SettingsPage.BackgroundColours[(int) ConfigController.Config.GetValue("Settings", "Background", 0)];

        ResourceLoader.LoadThreadedRequest(Path, useSubThreads:true);

    }
    public override void _Process(double delta)
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
