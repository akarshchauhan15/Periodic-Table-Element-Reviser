using Godot;

public partial class SettingsPage : Control
{
    ScrollContainer SwipeContainer;
    Vector2 StartPosition;
    bool IsDragging = false;
    int PageCount = 0;
    int CurrentPage = 0;
    float PageWidth;
    public override void _Ready()
    {
        GetNode<Button>("ExitButton").Pressed += OnExitPressed;
        SwipeContainer = GetNode<ScrollContainer>("SwipeContainer");

        PageCount = SwipeContainer.GetChild(0).GetChildCount();
        PageWidth = SwipeContainer.Size.X;
    }
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
                float Delta = Touch.Position.X - StartPosition.X;
                GD.Print(Delta);
                Delta /= 500;
                if (Delta < -1) CurrentPage++;
                else if (Delta > 1) CurrentPage--;

                CurrentPage = Mathf.Clamp(CurrentPage, 0, PageCount - 1);
                SnapToPage(CurrentPage);
            }
        }
    }
    private void SnapToPage(int TargetPage)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(SwipeContainer, "scroll_horizontal", TargetPage * PageWidth, 0.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Sine);
    }
    private void OnExitPressed() => GetParent<Hud>().AnimatePages(this, Hud.Pages[0]);
}