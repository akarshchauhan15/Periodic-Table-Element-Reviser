using Godot;

public partial class ConfirmationPage : Control
{
    static Label Given;
    static Label Return;

    public override void _Ready()
    {
        Given = GetNode<Label>("GivenLabel");
        Return = GetNode<Label>("ReturnLabel");

        GetNode<Button>("ContinueButton").Pressed += ProceedToAction;
    }
    public static void SetLabels()
    {
        Given.Text = Element.OptionValues[SeletionPage.GivenIndex];
        Return.Text = Element.OptionValues[SeletionPage.ReturnIndex];
    }
    private void ProceedToAction()
    {
        Hud.ContinuePage(this);
        GetNode<ActionPage>("../ActionPage").Initialize();
    }
}
