using Godot;

public partial class ResultPage : Control
{
    Label ScoreLabel;
    Label TimeTaken;

    public override void _Ready()
    {
        ScoreLabel = GetNode<Label>("Score");
        TimeTaken = GetNode<Label>("TimeTaken");
    }
    public void SetResults()
    {
        ActionPage Action = GetNode<ActionPage>("../ActionPage");
        ScoreLabel.Text = $"{Action.Score} / {Action.Length}";
        TimeTaken.Text = Action.TimeElapsed.ToString().PadDecimals(0).PadZeros(2) ;
    }
}
