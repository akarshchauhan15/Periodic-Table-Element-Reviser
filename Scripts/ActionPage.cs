using Godot;
using System;
using System.Collections.Generic;

public partial class ActionPage : Control
{
    Label DisplayLabel;
    Label GivenValueLabel;
    LineEdit InputValue;

    List<Element> ElementList;

    public int Score = 0;

    public override void _Ready()
    {
        DisplayLabel = GetNode<Label>("DisplayLabel");
        GivenValueLabel = GetNode<Label>("GivenValueLabel");
        InputValue = GetNode<LineEdit>("LineEdit");

        InputValue.TextSubmitted += GetInput;
    }
    public void Initialize()
    {
        DisplayLabel.Text = Element.OptionValues[SeletionPage.GivenIndex];
        InputValue.PlaceholderText = Element.OptionValues[SeletionPage.ReturnIndex];

        ElementList = Elements.GetElementList();
        GiveValue();
    }
    private void GiveValue()
    {
        if (ElementList.Count == 0) 
        {
            GD.Print("Complete");
            GD.Print(Score);
            return;
        }

        GivenValueLabel.Text = ElementList[0].Get(SeletionPage.SelectedGivenOption).ToString();
    }
    private void GetInput(string Input)
    {
        if (Input == ElementList[0].Get(SeletionPage.SelectedReturnOption).ToString())
        {
            Score++;
        }
        ElementList.RemoveAt(0);
        GiveValue();
    }
}
