using Godot;
using Godot.Collections;
using System;

public partial class Hud : Control
{
    Array<Control> Pages = new Array<Control>();
    Tween tween;
    Control Blank;

    public static AudioStreamPlayer UISelectAudio;
    public override void _Ready()
    {
        string[] PageNames = ["Home", "Selection", "Collection", "Confirmation", "Action", "Result"];

        foreach (string PageName in PageNames)
            Pages.Add(GetNode<Control>($"{PageName}Page"));

        Blank = GetNode<Control>("BlankPreventer");
        Blank.Show();
        UISelectAudio = GetNode<AudioStreamPlayer>("Audio/UI");
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
        UISelectAudio.Play();

        tween = CreateTween();

        tween.SetParallel(true);
        tween.TweenCallback(Callable.From(() => Blank.MouseFilter = MouseFilterEnum.Stop));
        tween.TweenProperty(FromPage, "modulate:a", 0, SettingsPage.AnimationFadeDuration).From(1);

        tween.TweenCallback(Callable.From(() => ToPage.Show())).SetDelay(SettingsPage.AnimationFadeDuration / 2);
        tween.TweenProperty(ToPage, "modulate:a", 1,SettingsPage.AnimationFadeDuration).From(0).SetDelay(SettingsPage.AnimationFadeDuration/2);

        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(() => Blank.MouseFilter = MouseFilterEnum.Ignore));
        tween.TweenCallback(Callable.From(() => FromPage.Hide()));
    }
}