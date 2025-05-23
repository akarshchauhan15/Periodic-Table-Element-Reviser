using Godot;
using Godot.Collections;
using System.IO;

public partial class ScreenLoader : Control
{
    TextureRect ProgressRect;
    Timer timer;
    string Path = "res://Scenes/hud.tscn";
    float DefaultProgressRectX;
    bool LoadingStarted = false;

    Array Progress = new Array();
    public override void _Ready()
    {
        ProgressRect = GetNode<TextureRect>("ProgressRect");
        GetNode<Timer>("HoldTimer").Timeout += HoldTimerTimeout;

        DefaultProgressRectX = ProgressRect.Size.X;
        ProgressRect.Size = new Vector2(0, ProgressRect.Size.Y);

        GetNode<ColorRect>("../Background").Color = SettingsPage.BackgroundColours[(int) ConfigController.Config.GetValue("Settings", "Background", 0)];
    }
    public override void _Process(double delta)
    {
        if (!LoadingStarted)
            return;
        ResourceLoader.LoadThreadedGetStatus(Path, Progress);

        ProgressRect.Size = new Vector2((float)Progress[0] * DefaultProgressRectX, ProgressRect.Size.Y);

        if ((double)Progress[0] >= 1)
        {
            PackedScene HudScene = (PackedScene) ResourceLoader.LoadThreadedGet(Path);
            GetParent().AddChild(HudScene.Instantiate<Hud>());
            QueueFree();
        }
    }
    private void HoldTimerTimeout() 
    {
        ResourceLoader.LoadThreadedRequest(Path, "", true);
        LoadingStarted = true;
    }
    
}
