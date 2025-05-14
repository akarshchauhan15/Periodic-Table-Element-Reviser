using Godot;

public partial class ConfirmationPage : Control
{
    Label Given;
    Label Return;
    Label Elements;

    SelectionPage Selection;

    public override void _Ready()
    {
        Given = GetNode<Label>("GivenLabel");
        Return = GetNode<Label>("ReturnLabel");
        Elements = GetNode<Label>("ElementsLabel");

        GetNode<Button>("ContinueButton").Pressed += ProceedToAction;
        GetNode<Button>("BackButton").Pressed += BackToCollection;

        Selection = GetNode<SelectionPage>("../SelectionPage");
    }
    public void SetLabels()
    {
        Given.Text = Element.OptionValues[Selection.GivenIndex];
        Return.Text = Element.OptionValues[Selection.ReturnIndex];
        Elements.Text = $"From {CollectionPage.SelectedCollection.DisplayName.ToLower()} elements";
    }
    private void ProceedToAction()
    {
        GetNode<ActionPage>("../ActionPage").Initialize();
        Hud.ContinuePage(this);
    }
    private void BackToCollection() => Hud.PreviousPage(this);
}
