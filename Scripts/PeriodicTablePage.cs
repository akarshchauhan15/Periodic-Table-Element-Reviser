using Godot;

public partial class PeriodicTablePage : Control
{
    Control TableElementContainer;
    Control Selected;
    PackedScene TableElementScene;

    Panel SelectedTableElement;
    public override void _Ready()
    {
        TableElementContainer = GetNode<Control>("ScrollContainer/Control");
        Selected = GetNode<Control>("Selected");
        TableElementScene = ResourceLoader.Load<PackedScene>("res://Scenes/table_element.tscn");

        GetNode<Button>("BackButton").Pressed += ReturnToHome;

        SetTable();
    }
    private void SetTable()
    {
        int Counter = 0;
        foreach (Element element in Elements.ElementList)
        {
            Panel TableElement = TableElementScene.Instantiate<Panel>();

            int ChildCount = 4;
            int Index = 0;
            while (Index < ChildCount)
            {
                TableElement.GetChild<Label>(Index).Text = element.Get(Element.Properties[Index]).ToString();
                Index++;
            }

            TableElement.GetNode<Button>("Button").Pressed += () =>
            {
                if (SelectedTableElement != TableElement)
                    SelectElement(TableElement);
            };   

            var GridPosition = Elements.ElementGridPositions[Counter];
            TableElement.Position = new Vector2(40 + (GridPosition.column * 160), 40 + (GridPosition.row * 160));
            TableElementContainer.AddChild(TableElement);

            Counter++;
        }
    }
    private void SelectElement(Panel TableElement)
    {
        SelectedTableElement = TableElement;

        int ChildCount = 4;
        int Index = 0;
        while (Index < ChildCount)
        {
            Selected.GetChild<Label>(Index).Text = TableElement.GetChild<Label>(Index).Text;
            Index++;
        }
    }
    private void ReturnToHome() => GetParent<Hud>().AnimatePages(this, GetNode<HomePage>("../HomePage"));
}
