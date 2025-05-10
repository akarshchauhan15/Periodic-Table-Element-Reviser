using Godot;

public partial class ConfirmationPage : Control
{
    Label Given;
    Label Return;

    SelectionPage Selection;

    public override void _Ready()
    {
        Given = GetNode<Label>("GivenLabel");
        Return = GetNode<Label>("ReturnLabel");

        GetNode<Button>("ContinueButton").Pressed += ProceedToAction;
        Selection = GetNode<SelectionPage>("../SelectionPage");
    }
    public void SetLabels()
    {
        Given.Text = Element.OptionValues[Selection.GivenIndex];
        Return.Text = Element.OptionValues[Selection.ReturnIndex];
    }
    private void ProceedToAction()
    {
        Hud.ContinuePage(this);
        GetNode<ActionPage>("../ActionPage").Initialize();
    }
}
