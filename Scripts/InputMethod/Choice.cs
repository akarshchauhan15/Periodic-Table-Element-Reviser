using Godot;
using Godot.Collections;

public partial class Choice : Control
{
    ActionPage Action;
    GridContainer ButtonContainer;
    string CorrectValue;

    public override void _Ready()
    {
        Action = GetNode<ActionPage>("../../");
        ButtonContainer = GetNode<GridContainer>("ButtonContainer");

        int Count = 1;
        foreach (Button ChoiceButton in ButtonContainer.GetChildren())
        {
            ChoiceButton.Pressed += () => ButtonPressed(ChoiceButton);    
            Count++;
        }
    }
    public void Initialize()
    {
        Show();
    }
    public void OnNewQuestionArrival()
    {
        SetOptionValues();
    }
    private void ButtonPressed(Button ChoiceButton)
    {
        bool Correct = false;

        if (ChoiceButton.Text == CorrectValue) Correct = true;

        Action.AddScore(Correct, ChoiceButton.Text); 
    }
    private void SetOptionValues()
    {
        Array<string> RandomValues = new Array<string>();
        Array<Element> ElementList = Action.ElementList.Duplicate();

        CorrectValue = ElementList[Action.Counter].Get(Action.Selection.SelectedReturnOption).ToString();
        RandomValues.Add(CorrectValue);
        ElementList.RemoveAt(Action.Counter);

        for (int i = 0; i<3; i++)
        {
            Element RandomElement = ElementList.PickRandom();
            RandomValues.Add(RandomElement.Get(Action.Selection.SelectedReturnOption).ToString());
            ElementList.Remove(RandomElement);
        }

        RandomValues.Shuffle();
        int Option = 0;

        foreach (Button ChoiceButton in ButtonContainer.GetChildren())
        {
            ChoiceButton.Text = RandomValues[Option];
            Option++;
        }
    }
}
