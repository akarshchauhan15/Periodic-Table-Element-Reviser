using Godot;
using Godot.Collections;

public partial class CollectionPage : Control
{
    OptionButton Type;
    OptionButton Collection;
    public Array<ElementCollection> SelectedType;
    public static ElementCollection SelectedCollection;

    public override void _Ready()
    {
        Type = GetNode<OptionButton>("Type");
        Collection = GetNode<OptionButton>("Collection");

        Type.ItemSelected += OnTypeSelected;
        Collection.ItemSelected += OnCollectionSelected;
        GetNode<Button>("ContinueButton").Pressed += ProceedToConfirmation;
        GetNode<Button>("BackButton").Pressed += BackToSelection;
    }
    public void LoadValues()
    {
        int LastType = (int) ConfigController.Config.GetValue("LastSelected", "ElementCollectionType", 0);
        OnTypeSelected(LastType);
        Type.Select(LastType);

        Collection.Select((int)ConfigController.Config.GetValue("LastSelected", "ElementCollectionList", 0));
    }
    private void OnTypeSelected(long Index)
    {
        SelectedType = ElementCollections.CollectionArray[(int)Index];

        Collection.Clear();
        foreach (ElementCollection elementCollection in SelectedType)   
            Collection.AddItem(elementCollection.Get(ElementCollection.PropertyName.DisplayName).ToString());

        Collection.Select(0);
        Hud.UISelectAudio.Play();
    }
    private void ProceedToConfirmation()
    {
        ConfigController.SaveSettings("LastSelected", "ElementCollectionType", Type.Selected);
        ConfigController.SaveSettings("LastSelected", "ElementCollectionList", Collection.Selected);

        SelectedCollection = SelectedType[Collection.Selected];
        GetNode<ConfirmationPage>("../ConfirmationPage").SetLabels();
        GetParent<Hud>().ContinuePage(this);  
    }
    private void OnCollectionSelected(long Index) => Hud.UISelectAudio.Play();
    private void BackToSelection() => GetParent<Hud>().PreviousPage(this);
}
