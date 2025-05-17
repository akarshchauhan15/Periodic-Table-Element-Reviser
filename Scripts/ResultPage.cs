using Godot;

public partial class ResultPage : Control
{
    Label ScoreLabel;
    Label TimeTaken;
    PackedScene ScoreListElementScene;
    VBoxContainer ScoreListElementContainer;

    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("Score");
        TimeTaken = GetNode<Label>("TimeTaken");
        ScoreListElementScene = ResourceLoader.Load<PackedScene>("res://Scenes/score_list_element.tscn");
        ScoreListElementContainer = GetNode<VBoxContainer>("ScrollContainer/VBoxContainer");

        GetNode<Button>("ContinueButton").Pressed += OnContinueButtonPressed;
        GetNode<Button>("RetryButton").Pressed += OnRetryButtonPressed;
    }

    public void SetResults()
    {
        foreach (Panel Child in ScoreListElementContainer.GetChildren())
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
                ScoreElement.Set("", Colors.Red);
                WrongCounter++;
            }
            ScoreListElementContainer.AddChild(ScoreElement);
        }
    }
    private void OnContinueButtonPressed()
    {
        Hide();
        GetNode<SelectionPage>("../SelectionPage").Show();
    }
    private void OnRetryButtonPressed()
    {
        GetNode<ActionPage>("../ActionPage").Initialize();
        Hide();
        GetNode<ActionPage>("../ActionPage").Show();
    }
}
