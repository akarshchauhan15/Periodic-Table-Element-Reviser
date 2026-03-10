using System;
using Godot;
using Godot.Collections;

public partial class ActionPage : Control
{

    public enum InputType { TypeInto, MultipleChoice}

    Label DisplayLabel;
    Label GivenValueLabel;
    Label Progress;
    public Label TimeLabel;
    Button ExitButton;
    Panel Tip;
    Tween tween;

    public Array<Element> ElementList;
    public Array<bool> ElementCorrect = new Array<bool>();
    public Array<string> WrongReturns = new Array<string>();
    public SelectionPage Selection;

    public InputType InputMethod;
    public int Score = 0;
    public static bool isPlaying = false;
    public double TimeElapsed = 0;
    public int Counter = 0;
    public int Length = 0;
    bool ExitButtonPressed = false;
    double ExitTime = 0;

    public override void _Ready()
    {
        DisplayLabel = GetNode<Label>("DisplayLabel");
        GivenValueLabel = GetNode<Label>("GivenValueLabel");
        Progress = GetNode<Label>("Progress");
        TimeLabel = GetNode<Label>("TimeElapsed");
        ExitButton = GetNode<Button>("ExitButton");
        Tip = GetNode<Panel>("ExitTip");

        ExitButton.ButtonDown += ExitButtonDown;
        ExitButton.ButtonUp += () => ExitTime = 0;

        Selection = GetNode<SelectionPage>("../SelectionPage");
    }
    public override void _Process(double delta)
    {
        if (!isPlaying)
            return;

        TimeElapsed += delta;
        TimeLabel.Text = $"{(TimeElapsed / 60).ToString().PadDecimals(0).PadZeros(2)} : {(TimeElapsed % 60).ToString().PadDecimals(0).PadZeros(2)}";

        if (ExitTime != 0 && (TimeElapsed - ExitTime > 0.3))    
            EndGame();
    }
    public void Initialize()
    {
        DisplayLabel.Text = Element.OptionValues[Selection.GivenIndex];

        ElementList = CollectionPage.SelectedCollection.GetElementsList();
        ElementList.Shuffle();

        Tip.Modulate = new Color(1, 1, 1, 0);

        Counter = 0;
        Score = 0;
        ElementCorrect = [];
        WrongReturns = [];
        TimeElapsed = 0;
        Length = ElementList.Count;
        isPlaying = true;

        CallFunctionOnInputHandler("Initialize");
        GiveValue();
    }
        public void AddScore(bool Correct, string Answer)
    {
        if (Correct)
        {
            Score++;
            ElementCorrect.Add(true);
        }
        else
        {
            WrongReturns.Add(Answer);
            ElementCorrect.Add(false);
        }

        Counter++;
        GiveValue();  
    }
    private void CallFunctionOnInputHandler(String Function)
    {
        switch (InputMethod)
        {
            case InputType.TypeInto:
                GetNode<Text>("InputInterface/Text").Call(Function);
                return;
            case InputType.MultipleChoice:
                GetNode<Text>("InputInterface/Text").Call(Function);
                return;
        }
    }
    private void GiveValue()
    {
        if (Counter >= Length) { EndGame(); return; }

        Godot.Input.VibrateHandheld(20, Settings.VibrationFeedbackIntensity);
        GivenValueLabel.Text = ElementList[Counter].Get(Selection.SelectedGivenOption).ToString();
        Progress.Text = $"{Counter + 1} / {Length}";

        CallFunctionOnInputHandler("OnNewQuestionArrival");
    }
    private void EndGame()
    {
        isPlaying = false;
        GetNode<ResultPage>("../ResultPage").SetResults();
        GetParent<Hud>().ContinuePage(this);
    }
    private void ExitButtonDown()
    {
        ExitTime = TimeElapsed;

        if (tween != null)
            tween.Kill();

        tween = CreateTween();

        tween.TweenProperty(Tip, "modulate:a", 1, 0.5 * (1 - Tip.Modulate.A));
        tween.TweenInterval(1.5f);
        tween.TweenProperty(Tip, "modulate:a", 0, 0.5);
    }
}
