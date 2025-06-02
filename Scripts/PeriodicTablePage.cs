using Godot;
using System;

public partial class PeriodicTablePage : Control
{
    Control TableElementContainer;
    PackedScene TableElementScene;


    public override void _Ready()
    {
        TableElementContainer = GetNode<Control>("ScrollContainer/Control");
        TableElementScene = ResourceLoader.Load<PackedScene>("res://Scenes/table_element.tscn");
        SetTable();
    }
    private void SetTable()
    {
        foreach (Element element in Elements.ElementList)
        {
            Panel TableElement = TableElementScene.Instantiate<Panel>();

            int ChildCount = TableElement.GetChildCount();
            int Index = 0;
            while (Index < ChildCount)
            {
                TableElement.GetChild<Label>(Index).Text = element.Get(Element.Properties[Index]).ToString();
                Index++;
            }

            TableElementContainer.AddChild(TableElement);
        }
    } 
}
