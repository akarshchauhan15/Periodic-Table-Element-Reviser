using Godot;
using Godot.Collections;
using System.Linq.Expressions;

public partial class ActionPage : Control
{
    Label DisplayLabel;
    Label GivenValueLabel;
    LineEdit InputValue;
    Label Progress;
    public Label TimeLabel;
    Timer KeyboardTimer;
    Button ExitButton;
    Panel Tip;

    public Array<Element> ElementList;
    public Array<bool> ElementCorrect = new Array<bool>();
    public Array<string> WrongReturns = new Array<string>();
    SelectionPage Selection;

    LineEdit.VirtualKeyboardTypeEnum KeyboardType;
    public int Score = 0;
    public static bool isPlaying = false;
    public double TimeElapsed = 0;
    public int Counter = 0;
    public int Length = 0;
    bool ExitButtonPressed = false;
    double ExitTime = 0;
    Tween tween;

    public override void _Ready()
    {
        DisplayLabel = GetNode<Label>("DisplayLabel");
        GivenValueLabel = GetNode<Label>("GivenValueLabel");
        InputValue = GetNode<LineEdit>("LineEdit");
        Progress = GetNode<Label>("Progress");
        TimeLabel = GetNode<Label>("TimeElapsed");
        KeyboardTimer = GetNode<Timer>("KeyboardTimer");
        ExitButton = GetNode<Button>("ExitButton");
        Tip = GetNode<Panel>("ExitTip");

        InputValue.TextSubmitted += GetInput;
        KeyboardTimer.Timeout += OnKeyboardTimerEnds;
        ExitButton.ButtonDown += ExitButtonDown;
        ExitButton.ButtonUp += ExitButtonUp;

        Selection = GetNode<SelectionPage>("../SelectionPage");
    }
    public override void _Process(double delta)
    {
        if (!isPlaying)
            return;

        TimeElapsed += delta;
        TimeLabel.Text = $"{(TimeElapsed / 60).ToString().PadDecimals(0).PadZeros(2)} : {(TimeElapsed % 60).ToString().PadDecimals(0).PadZeros(2)}";

        if (ExitTime != 0 && (TimeElapsed - ExitTime > 0.5))    
            EndGame();
    }
    public void Initialize()
    {
        DisplayLabel.Text = Element.OptionValues[Selection.GivenIndex];

        ElementList = CollectionPage.SelectedCollection.GetElementsList();
        ElementList.Shuffle();

        InputValue.PlaceholderText = Element.OptionValues[Selection.ReturnIndex];

        if (Selection.SelectedReturnOption == Element.PropertyName.AtomicNumber || Selection.SelectedReturnOption == Element.PropertyName.AtomicMass)
            KeyboardType = LineEdit.VirtualKeyboardTypeEnum.NumberDecimal;
        else
            KeyboardType = LineEdit.VirtualKeyboardTypeEnum.Default;

        InputValue.VirtualKeyboardType = KeyboardType;
        if (tween != null)
            tween.Kill();

        Tip.Modulate = new Color(1, 1, 1, 0);

        Counter = 0;
        Score = 0;
        ElementCorrect = [];
        WrongReturns = [];
        TimeElapsed = 0;
        InputValue.Text = "";
        Length = ElementList.Count;
        isPlaying = true;

        GiveValue();
    }
    private void GiveValue()
    {
        if (Counter >= Length)
        {
            EndGame();
            return;
        }

        GivenValueLabel.Text = ElementList[Counter].Get(Selection.SelectedGivenOption).ToString();
        Progress.Text = $"{Counter + 1} / {Length}";
        KeyboardTimer.Start();
    }
    private void GetInput(string Input)
    {
        if (Input == "")
            return;

        if (Input == ElementList[Counter].Get(Selection.SelectedReturnOption).ToString())
        {
            Score++;
            ElementCorrect.Add(true);
        }
        else
        {
            WrongReturns.Add(Input);
            ElementCorrect.Add(false);
        }

        InputValue.Text = "";
        Counter++;

        InputValue.Unedit();
        GiveValue();
    }
    private void EndGame()
    {
        isPlaying = false;
        GetNode<ResultPage>("../ResultPage").SetResults();
        GetParent<Hud>().ContinuePage(this);
    }
    private void OnKeyboardTimerEnds() {
        InputValue.GrabFocus();
        InputValue.Edit();
    }
    private void ExitButtonDown()
    {
        ExitTime = TimeElapsed;
        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(Tip, "modulate:a", 1, 0.5 * (1 - Tip.Modulate.A));
        tween.TweenInterval(1.5);
        tween.TweenProperty(Tip, "modulate:a", 0, 0.5 * (1 - Tip.Modulate.A));
    }
    private void ExitButtonUp() => ExitTime = 0;
}
