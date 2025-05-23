using Godot;
using System.Text.RegularExpressions;

public partial class ResultPage : Control
{
    Label ScoreLabel;
    Label TimeTaken;

    VBoxContainer AllScoreListElementContainer;
    VBoxContainer WrongScoreListElementContainer;

    PackedScene ScoreListElementScene;
    PackedScene ListIndicatorScene;
    PackedScene GreatPanelScene;

    bool AllCorrect;
    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("Score");
        TimeTaken = GetNode<Label>("TimeTaken");
        
        AllScoreListElementContainer = GetNode<VBoxContainer>("AllElements/VBoxContainer");
        WrongScoreListElementContainer = GetNode<VBoxContainer>("WrongElements/VBoxContainer");

        ScoreListElementScene = ResourceLoader.Load<PackedScene>("res://Scenes/score_list_element.tscn");
        ListIndicatorScene = ResourceLoader.Load<PackedScene>("res://Scenes/list_indicator.tscn");
        GreatPanelScene = ResourceLoader.Load<PackedScene>("res://Scenes/great_panel.tscn");

        GetNode<Button>("ToggleButton").Pressed += OnListToggled;
        GetNode<Button>("ContinueButton").Pressed += OnContinueButtonPressed;
        GetNode<Button>("RetryButton").Pressed += OnRetryButtonPressed;
    }

    public void SetResults()
    {
        foreach (Control Child in AllScoreListElementContainer.GetChildren())
            Child.QueueFree();

        foreach (Control Child in WrongScoreListElementContainer.GetChildren())
            Child.QueueFree();

        ActionPage Action = GetNode<ActionPage>("../ActionPage");
        SelectionPage Selection = GetNode<SelectionPage>("../SelectionPage");

        ScoreLabel.Text = $"{Action.Score} / {Action.Length}";
        TimeTaken.Text = Action.TimeLabel.Text;

        int WrongCounter = 0;

        for (int i = 0; i < Action.ElementCorrect.Count; i++)
        {
            Panel ScoreElement = ScoreListElementScene.Instantiate<Panel>();

            ScoreElement.GetNode<Label>("Given").Text = Action.ElementList[i].Get(Selection.SelectedGivenOption).ToString();

            if (Action.ElementCorrect[i])
            {
                ScoreElement.GetNode<Label>("Return").Text = Action.ElementList[i].Get(Selection.SelectedReturnOption).ToString();
                ScoreElement.GetNode<Label>("Return").Show();
            }
            else
            {
                ScoreElement.GetNode<Label>("Wrong").Text = Action.WrongReturns[WrongCounter];
                ScoreElement.GetNode<Label>("Wrong/Correct").Text = Action.ElementList[i].Get(Selection.SelectedReturnOption).ToString();
                ScoreElement.GetNode<Label>("Wrong").Show();

                WrongScoreListElementContainer.AddChild(ScoreElement.Duplicate());

                WrongCounter++;
            }
            AllScoreListElementContainer.AddChild(ScoreElement);
        }

        if (WrongCounter == 0)
        WrongScoreListElementContainer.AddChild(GreatPanelScene.Instantiate<Panel>());

        AllScoreListElementContainer.AddChild(ListIndicatorScene.Instantiate<Label>());
        ((Label)AllScoreListElementContainer.GetChild(-1)).Text = "All Elements";

        WrongScoreListElementContainer.AddChild(ListIndicatorScene.Instantiate<Label>());
        ((Label)WrongScoreListElementContainer.GetChild(-1)).Text = "Incorrect Filtered";

        WrongScoreListElementContainer.Hide();
        AllScoreListElementContainer.Show();
    }
    private void OnListToggled()
    {
        AllScoreListElementContainer.Visible = !AllScoreListElementContainer.Visible;
        WrongScoreListElementContainer.Visible = !WrongScoreListElementContainer.Visible;
    }
    private void OnContinueButtonPressed() => GetParent<Hud>().AnimatePages(this, GetNode<SelectionPage>("../SelectionPage"));
    private void OnRetryButtonPressed()
    {
        GetNode<ActionPage>("../ActionPage").Initialize();
        GetParent<Hud>().PreviousPage(this);
    }
}
