using Godot;

public partial class SelectionPage : Control
{
    OptionButton GivenOption;
    OptionButton ReturnOption;
    public int GivenIndex;
    public int ReturnIndex;
    public StringName SelectedGivenOption;
    public StringName SelectedReturnOption;

    public override void _Ready()
    {
        GivenOption = GetNode<OptionButton>("GivenOption");
        ReturnOption = GetNode<OptionButton>("ReturnOption");

        GivenOption.ItemSelected += SelectGivenOption;
        ReturnOption.ItemSelected += SelectReturnOption;
        GetNode<Button>("ContinueButton").Pressed += ProceedToCollection;
        GetNode<Button>("BackButton").Pressed += BackToHome;
    }
    public void LoadValues()
    {
        SelectGivenOption((long) ConfigController.Config.GetValue("LastSelected", "Given", 0));
        SelectReturnOption((long)ConfigController.Config.GetValue("LastSelected", "Return", 2));

        ReturnOption.Select(ReturnIndex);
        GivenOption.Select(GivenIndex);
    }
    private void SelectGivenOption(long Index)
    {
        GivenIndex = (int) Index;
        SelectedGivenOption = SetValueToVariable(Index);

        for (int i = 0; i < 4; i++)
        {
            if (i == (int)Index)
            {
                ReturnOption.SetItemDisabled(i, true);
                continue;
            }
            ReturnOption.SetItemDisabled(i, false);
        }
        if (ReturnOption.GetSelectedId() != Index)
            return;

        ReturnOption.Select(((int)Index + 1) % 4);
        SelectReturnOption((Index + 1) % 4);
    }
    private void SelectReturnOption(long Index)
    {
        ReturnIndex = (int) Index;
        SelectedReturnOption = SetValueToVariable(Index);
    }
    private StringName SetValueToVariable(long Index)
    {
        switch (Index)
        {
            case 0:
                return Element.PropertyName.Name;
            case 1:
                return Element.PropertyName.Symbol;
            case 2:
                return Element.PropertyName.AtomicNumber;
            case 3:
                return Element.PropertyName.AtomicMass;
            default:
                return Element.PropertyName.Name;
        }
    }
    private void ProceedToCollection()
    {
        ConfigController.SaveSettings("LastSelected", "Given", GivenIndex);
        ConfigController.SaveSettings("LastSelected", "Return", ReturnIndex);

        GetNode<CollectionPage>("../CollectionPage").LoadValues();
        Hud.ContinuePage(this);
    }
    private void BackToHome() => Hud.PreviousPage(this);
}
