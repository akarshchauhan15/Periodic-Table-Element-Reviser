using System;
using Godot;

public partial class ConfirmationPage : Control
{
    Label Given;
    Label Return;
    Label Elements;

    SelectionPage Selection;
    ActionPage Action;

    OptionButton InputOption;

    public override void _Ready()
    {
        Given = GetNode<Label>("GivenLabel");
        Return = GetNode<Label>("ReturnLabel");
        Elements = GetNode<Label>("ElementsLabel");

        InputOption = GetNode<OptionButton>("InputOption");

        InputOption.ItemSelected += (long Index) => { Hud.SendSoundAndFeedback(); };
        InputOption.Pressed += () => Hud.SendSoundAndFeedback();

        GetNode<Button>("ContinueButton").Pressed += ProceedToAction;
        GetNode<Button>("BackButton").Pressed += BackToCollection;

        Selection = GetNode<SelectionPage>("../SelectionPage");
        Action = GetNode<ActionPage>("../ActionPage");
    }
    public void SetParameters()
    {
        Given.Text = Element.OptionValues[Selection.GivenIndex];
        Return.Text = Element.OptionValues[Selection.ReturnIndex];
        Elements.Text = $"From {CollectionPage.SelectedCollection.DisplayName.ToLower()} elements";

        InputOption.Select((int)ConfigController.Config.GetValue("LastSelected", "InputMethod", 0));
    }
    public void ProceedToAction()
    {
        ConfigController.SaveSettings("LastSelected", "InputMethod", InputOption.Selected);

        Action.InputMethod = (ActionPage.InputType)InputOption.Selected;
        Action.Initialize();
        GetParent<Hud>().ContinuePage(this);
    }
    private void BackToCollection() => GetParent<Hud>().PreviousPage(this);
}
