using Godot;
using System;
using System.Reflection;

public partial class SeletionPage : Control
{
    OptionButton GivenOption;
    OptionButton ReturnOption;
    public static int GivenIndex;
    public static int ReturnIndex;
    public static StringName SelectedGivenOption;
    public static StringName SelectedReturnOption;

    public override void _Ready()
    {
        GivenOption = GetNode<OptionButton>("GivenOption");
        ReturnOption = GetNode<OptionButton>("ReturnOption");

        GivenOption.ItemSelected += SelectGivenOption;
        ReturnOption.ItemSelected += SelectReturnOption;
        GetNode<Button>("ContinueButton").Pressed += ProceedToConfirmation;
    }
    public void LoadValues()
    {
        GivenIndex = (int) ConfigController.Config.GetValue("LastSelected", "Given");
        ReturnIndex = (int) ConfigController.Config.GetValue("LastSelected", "Return");

        GivenOption.Select(GivenIndex);
        ReturnOption.SetItemDisabled(GivenIndex, true);
        ReturnOption.Select(ReturnIndex);
    }
    private void SelectGivenOption(long Index)
    {
        GivenIndex = (int) Index;
        SetValueToVariable(SelectedGivenOption, Index);

        for (int i = 0; i < 4; i++)
        {
            if (i == (int)Index)
            {
                ReturnOption.SetItemDisabled(i, true);
                continue;
            }
            ReturnOption.SetItemDisabled(i, false);
        }

        if (ReturnOption.GetSelectedId() == Index)     
            ReturnOption.Select(((int)Index+1)%4);
    }
    private void SelectReturnOption(long Index)
    {
        ReturnIndex = (int) Index;
        SetValueToVariable(SelectedReturnOption, Index);
    }
    private void SetValueToVariable(StringName SelectedVariable, long Index)
    {
        switch (Index)
        {
            case 0:
                SelectedVariable = Element.PropertyName.Name;
                break;
            case 1:
                SelectedVariable = Element.PropertyName.Symbol;
                break;
            case 2:
                SelectedVariable = Element.PropertyName.AtomicNumber;
                break;
            case 3:
                SelectedVariable = Element.PropertyName.AtomicMass;
                break;
        }
    }
    private void ProceedToConfirmation()
    {
        ConfigController.SaveSettings("LastSelected", "Given", GivenIndex);
        ConfigController.SaveSettings("LastSelected", "Return", ReturnIndex);

        ConfirmationPage.SetLabels();
        Hud.ContinuePage(this);
    }
}
