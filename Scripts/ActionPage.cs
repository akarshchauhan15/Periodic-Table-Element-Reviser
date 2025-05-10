using Godot;
using Godot.Collections;
using System.ComponentModel.DataAnnotations;

public partial class ActionPage : Control
{
    Label DisplayLabel;
    Label GivenValueLabel;
    LineEdit InputValue;
    Label Progress;
    Label TimeLabel;

    Array<Element> ElementList;
    SelectionPage Selection;

    public int Score = 0;
    public static bool isPlaying = false;
    public double TimeElapsed = 0;
    public int Counter = 0;
    public int Length = 0;

    public override void _Ready()
    {
        DisplayLabel = GetNode<Label>("DisplayLabel");
        GivenValueLabel = GetNode<Label>("GivenValueLabel");
        InputValue = GetNode<LineEdit>("LineEdit");
        Progress = GetNode<Label>("Progress");
        TimeLabel = GetNode<Label>("TimeElapsed");

        InputValue.TextSubmitted += GetInput;
        Selection = GetNode<SelectionPage>("../SelectionPage");
    }
    public override void _Process(double delta)
    { 
        if (isPlaying)
        {
            TimeElapsed += delta;

            TimeLabel.Text = TimeElapsed.ToString().PadDecimals(0).PadZeros(2);
        }
    }
    public void Initialize()
    {
        DisplayLabel.Text = Element.OptionValues[Selection.GivenIndex];
      
        ElementList = Elements.ElementList;

        InputValue.GrabFocus();
        InputValue.PlaceholderText = Element.OptionValues[Selection.ReturnIndex];

        if (Selection.SelectedReturnOption == Element.PropertyName.AtomicNumber || Selection.SelectedReturnOption == Element.PropertyName.AtomicMass)
            InputValue.VirtualKeyboardType = LineEdit.VirtualKeyboardTypeEnum.NumberDecimal;
        else
            InputValue.VirtualKeyboardType = LineEdit.VirtualKeyboardTypeEnum.Default;

        Counter = 0;
        Length = ElementList.Count;
        isPlaying = true;
        GiveValue();
    }
    private void GiveValue()
    {
        if (Counter >= Length) 
        {
            isPlaying = false;
            Hud.ContinuePage(this);
            return;
        }

        GivenValueLabel.Text = ElementList[Counter].Get(Selection.SelectedGivenOption).ToString();
        Progress.Text = $"{Counter + 1} / {Length}";
    }
    private void GetInput(string Input)
    {
        if (Input == ElementList[Counter].Get(Selection.SelectedReturnOption).ToString())
        {
            Score++;
        }
        InputValue.Text = "";
        Counter++;
        GiveValue();
    }
}
