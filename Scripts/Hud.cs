using Godot;
using Godot.Collections;

public partial class Hud : Control
{
    public static Array<Control> Pages = new Array<Control>();
    Tween tween;
    Control Blank;

    public static AudioStreamPlayer UISelectAudio;
    public override void _Ready()
    {
        UISelectAudio = GetNode<AudioStreamPlayer>("Audio/UI");
        string[] PageNames = ["Home", "Selection", "Collection", "Confirmation", "Action", "Result"];

        foreach (string PageName in PageNames)
            Pages.Add(GetNode<Control>($"{PageName}Page"));

        foreach (Control Page in Pages)
        {
            Page.ZIndex = -1; 
            Page.Show();
            Page.Hide();
            Page.ZIndex = 0;
        }

        Blank = GetNode<Control>("BlankPreventer");
        Blank.Show();

        Pages[0].Show();    
    }
    public void ContinuePage(Control CurrentPage)
    {
        int CurrentIndex = Pages.IndexOf(CurrentPage);
        if (CurrentIndex >= Pages.Count - 1)
            return;
        Control NextPage = Pages[CurrentIndex + 1];
        
        AnimatePages(CurrentPage, NextPage);     
    }
    public void PreviousPage(Control CurrentPage)
    {
        int CurrentIndex = Pages.IndexOf(CurrentPage);
        if (CurrentIndex <= 0)
            return;
        Control PreviousPage = Pages[CurrentIndex - 1];

        AnimatePages(CurrentPage, PreviousPage);
    }
    public void AnimatePages(Control FromPage, Control ToPage)
    {
        SendSoundAndFeedback();

        tween = CreateTween();

        tween.SetParallel(true);
        tween.TweenCallback(Callable.From(() => Blank.MouseFilter = MouseFilterEnum.Stop));
        tween.TweenProperty(FromPage, "modulate:a", 0, Settings.AnimationFadeDuration).From(1);

        tween.TweenCallback(Callable.From(() => ToPage.Show()));
        tween.TweenProperty(ToPage, "modulate:a", 1, Settings.AnimationFadeDuration);

        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(() => { 
            Blank.MouseFilter = MouseFilterEnum.Ignore; 
            FromPage.Hide();
            }));
    }
    public static void SendSoundAndFeedback(int Duration = 30)
    {
        UISelectAudio.Play();
        Input.VibrateHandheld(Duration, Settings.VibrationFeedbackIntensity);
    }
}