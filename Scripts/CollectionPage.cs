using Godot;
using Godot.Collections;

public partial class CollectionPage : Control
{
    OptionButton Type;
    OptionButton Collection;
    public Dictionary<string, ElementCollection> CurrentList = new Dictionary<string, ElementCollection>();

    public override void _Ready()
    {
        Type = GetNode<OptionButton>("Type");
        Collection = GetNode<OptionButton>("Collection");

        Type.ItemSelected += OnTypeSelected;
        GetNode<Button>("ContinueButton").Pressed += ProceedToAction;
    }
    private void OnTypeSelected(long Index)
    {
        CurrentList = ElementCollections.CollectionArray[(int)Index];

        System.Collections.Generic.ICollection<string> TempList;
        TempList = ElementCollections.CollectionArray[(int)Index].Keys;

        Collection.Clear();
        foreach (string Key in TempList) 
        {
            Collection.AddItem(Key);
        }
        Collection.Select(0);
    }
    private void ProceedToAction()
    {
        Hud.ContinuePage(this);
        CurrentList[""]
        GetNode<ActionPage>("../ActionPage").Initialize();
    }
}
