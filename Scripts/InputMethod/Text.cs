using Godot;
using System;

public partial class Text : Control
{

    ActionPage Action;
    LineEdit InputValue;
    Timer KeyboardTimer;

    public override void _Ready()
    {
        Action = GetNode<ActionPage>("../../");
        InputValue = GetNode<LineEdit>("LineEdit");
        KeyboardTimer = GetNode<Timer>("KeyboardTimer");

        InputValue.TextSubmitted += GetInput;
        KeyboardTimer.Timeout += OnKeyboardTimerEnds;
    }
    public void Initialize()
    {
        Show();
        InputValue.Text = "";
        InputValue.PlaceholderText = Element.OptionValues[Action.Selection.ReturnIndex];

        LineEdit.VirtualKeyboardTypeEnum KeyboardType;
        if (Action.Selection.SelectedReturnOption == Element.PropertyName.AtomicNumber || Action.Selection.SelectedReturnOption == Element.PropertyName.AtomicMass)
            KeyboardType = LineEdit.VirtualKeyboardTypeEnum.NumberDecimal;
        else
            KeyboardType = LineEdit.VirtualKeyboardTypeEnum.Default;

        InputValue.VirtualKeyboardType = KeyboardType;
    }
    public void GetInput(string Input)
    {
        if (Input == "")
            return;

        bool Correct = false;

        if (Action.Selection.SelectedReturnOption == Element.PropertyName.AtomicNumber)
            Correct = Input.ToFloat() == (float)Action.ElementList[Action.Counter].Get(Action.Selection.SelectedReturnOption);
        else if (Action.Selection.SelectedReturnOption == Element.PropertyName.AtomicMass)
            Correct = Mathf.Abs(Input.ToFloat() - (float)Action.ElementList[Action.Counter].Get(Action.Selection.SelectedReturnOption)) < 0.4f;
        else
            Correct = Input.ToLower() == Action.ElementList[Action.Counter].Get(Action.Selection.SelectedReturnOption).ToString().ToLower();

        Action.AddScore(Correct, Input);        
        
        InputValue.Text = "";
        InputValue.Unedit();
    }
    public void OnNewQuestionArrival()
    {
        KeyboardTimer.Start();
    }
    private void OnKeyboardTimerEnds() 
    {
        InputValue.GrabFocus();
        InputValue.Edit();
    }
}
