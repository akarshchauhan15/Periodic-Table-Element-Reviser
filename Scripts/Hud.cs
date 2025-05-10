using Godot;
using Godot.Collections;

public partial class Hud : Control
{
    static Array<Control> Pages = new Array<Control>();

    public override void _Ready()
    {
        string[] PageNames = { "Home", "Selection", "Confirmation", "Action", "Result" };

        foreach (string PageName in PageNames)
            Pages.Add(GetNode<Control>($"{PageName}Page"));

    }
    public static void ContinuePage(Control CurrentPage)
    {
        int CurrentIndex = Pages.IndexOf(CurrentPage);
        if (CurrentIndex >= Pages.Count - 1)
            return;

        CurrentPage.Hide();
        Pages[CurrentIndex + 1].Show();
    }
    public static void PreviousPage(Control CurrentPage)
    {
        int CurrentIndex = Pages.IndexOf(CurrentPage);

        if (CurrentIndex <= 0)
            return;

        CurrentPage.Hide();
        Pages[CurrentIndex - 1].Show();
    }
}
