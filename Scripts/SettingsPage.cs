using Godot;

public partial class SettingsPage : Control
{
    ScrollContainer SwipeContainer;
    ScrollContainer LabelContainer;

    Vector2 StartPosition;
    bool IsDragging = false;
    //double DragTime = 0;
    int PageCount = 0;
    int CurrentPage = 0;

    float PageWidth;
    float LabelWidth;
    public override void _Ready()
    {
        GetNode<Button>("ExitButton").Pressed += OnExitPressed;
        SwipeContainer = GetNode<ScrollContainer>("SwipeContainer");
        LabelContainer = GetNode<ScrollContainer>("LabelContainer");

        PageCount = SwipeContainer.GetChild(0).GetChildCount();
        PageWidth = SwipeContainer.Size.X;
        LabelWidth = 800;
    }
    /*public override void _PhysicsProcess(double delta)
    {
        if (IsDragging) DragTime += delta;
    }*/
    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch Touch)
        {
            if (Touch.Pressed)
            {
                IsDragging = true;
                StartPosition = Touch.Position;
            }
            else if (IsDragging)
            {
                IsDragging = false;
<<<<<<< HEAD
                float Delta = (Touch.Position.X - StartPosition.X); //* (float) (1 + 1/(DragTime*100));
=======
                float Delta = (Touch.Position.X - StartPosition.X) * (float) (1 + 1/(DragTime*100));
>>>>>>> main
                Delta /= 300;

                if (Delta < -1) CurrentPage++;
                else if (Delta > 1) CurrentPage--;

                CurrentPage = Mathf.Clamp(CurrentPage, 0, PageCount - 1);
                SnapToPage(CurrentPage);

                //DragTime = 0;
            }
        }
    }
    public void ResetPosition()
    {
        SwipeContainer.ScrollHorizontal = 0;
        LabelContainer.ScrollHorizontal = 60;
    }
    private void SnapToPage(int TargetPage)
    {
        Tween tween = CreateTween();
        tween.SetParallel(true);
<<<<<<< HEAD
        tween.TweenCallback(Callable.From(() =>  LabelContainer.MouseFilter = MouseFilterEnum.Ignore));
        tween.TweenCallback(Callable.From(() =>  MouseFilter = MouseFilterEnum.Ignore));

        double Time = (100 + Mathf.Abs(LabelContainer.ScrollHorizontal - 60 - TargetPage * LabelWidth)) * 0.0008;

        tween.TweenProperty(LabelContainer, "scroll_horizontal", 60 + TargetPage * LabelWidth, Time).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(SwipeContainer, "scroll_horizontal", TargetPage * PageWidth, 0.6f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);

        tween.TweenCallback(Callable.From(() => LabelContainer.MouseFilter = MouseFilterEnum.Pass)).SetDelay(Time);
        tween.TweenCallback(Callable.From(() => MouseFilter = MouseFilterEnum.Pass)).SetDelay(Time);
=======
        tween.TweenProperty(LabelContainer, "scroll_horizontal", 60 + TargetPage * LabelWidth, (100 + Mathf.Abs(LabelContainer.ScrollHorizontal - 60 - TargetPage * LabelWidth)) * 0.0008).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(SwipeContainer, "scroll_horizontal", TargetPage * PageWidth, 0.6f).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic);
>>>>>>> main
    }
    private void OnExitPressed() => GetParent<Hud>().AnimatePages(this, Hud.Pages[0]);
}