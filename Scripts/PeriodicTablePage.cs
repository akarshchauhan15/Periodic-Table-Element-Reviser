using Godot;

public partial class PeriodicTablePage : Control
{
    Control TableElementContainer;
    Control Selected;
    PackedScene TableElementScene;

    Panel SelectedTableElement;

    Color[] CategoryColors = [
        new Color(0.169f, 0.22f, 0.391f),
        new Color(0.173f, 0.368f, 0.406f),
        new Color(0.316f, 0.458f, 0.535f),
        new Color(0.063f, 0.306f, 0.438f),
        new Color(0.314f, 0.209f, 0.352f),
        new Color(0.16f, 0.41f, 0.258f),
        new Color(0.293f, 0.348f, 0.535f),
        new Color(0.391f, 0.382f, 0.163f),
        new Color(0.353f, 0.239f, 0.52f),
        new Color(0.241f, 0.082f, 0.414f),
        new Color(0.25f, 0.158f, 0.539f)
        ];

    float[] ScaleValues = [0.611f, 0.72f, 0.873f, 1.0f, 1.17f, 1.37f, 1.55f, 1.72f];

    int TableScale = 3;
    public override void _Ready()
    {
        TableElementContainer = GetNode<Control>("ScrollContainer/Control");
        Selected = GetNode<Control>("Selected");
        TableElementScene = ResourceLoader.Load<PackedScene>("res://Scenes/table_element.tscn");

        GetNode<Button>("BackButton").Pressed += ReturnToHome;

        GetNode<Button>("ScaleController/IncreaseScale").Pressed += () => { TableScale = Mathf.Clamp(TableScale + 1, 0, ScaleValues.Length - 1); SetScale(); };
        GetNode<Button>("ScaleController/DecreaseScale").Pressed += () => { TableScale = Mathf.Clamp(TableScale - 1, 0, ScaleValues.Length - 1); SetScale(); };
        GetNode<Button>("ScaleController/ResetScale").Pressed += () => { TableScale = 3; SetScale(); };

        SetTable();
    }
    private void SetTable()
    {
        TableElementContainer.CustomMinimumSize = new Vector2(2950, 1660) * ScaleValues[TableScale];

        int Counter = 0;
        foreach (Element element in Elements.ElementList)
        {
            Panel TableElement = TableElementScene.Instantiate<Panel>();

            int ChildCount = 3;
            int Index = 0;

            while (Index < ChildCount)
            {
                TableElement.GetChild<Label>(Index).Text = element.Get(Element.Properties[Index]).ToString();
                Index++;
            }

            TableElement.GetNode<Label>("AtomicMass").Text = element.AtomicMass.ToString().PadDecimals(1);
            

            StyleBoxFlat Style = (StyleBoxFlat) GD.Load<StyleBoxFlat>("res://Themes/table_element.stylebox").Duplicate(); 

            Color CategoryColor = CategoryColors[(int)element.Category];
            Style.BgColor = CategoryColor;
            Style.BorderColor = CategoryColor.Darkened(0.25f);

            TableElement.AddThemeStyleboxOverride("panel", Style);

            TableElement.GetNode<Button>("Button").Pressed += () =>
            {
                if (SelectedTableElement != TableElement)
                    SelectElement(TableElement);
            };
            TableElement.SetMeta("category_color", CategoryColors[(int)element.Category]);

            SetPanelScale(TableElement, Counter);
            TableElementContainer.AddChild(TableElement);

            Counter++;
        }

        SelectElement(TableElementContainer.GetChild<Panel>(0));
    }
    private void SelectElement(Panel TableElement)
    {
        SelectedTableElement = TableElement;

        int ChildCount = 3;
        int Index = 0;
        while (Index < ChildCount)
        {
            Selected.GetChild<Label>(Index).Text = TableElement.GetChild<Label>(Index).Text;
            Index++;
        }


        Selected.GetNode<Label>("AtomicMass").Text = Elements.ElementList[TableElement.GetChild<Label>(2).Text.ToInt() - 1].AtomicMass.ToString();

        StyleBoxFlat Style = GD.Load<StyleBoxFlat>("res://Themes/panel_decorator.stylebox");

        Color StyleColor = (Color)TableElement.GetMeta("category_color", Colors.Transparent);
        Style.BgColor = StyleColor.Darkened(0.4f);
        Style.BorderColor = StyleColor.Lightened(0.1f);

        GetNode<Panel>("Selected/Name/Panel").AddThemeStyleboxOverride("panel", Style);
    }
    private void SetPanelScale(Panel TableElement, int Counter)
    {
        TableElement.Scale = Vector2.One * ScaleValues[TableScale];
        var GridPosition = Elements.ElementGridPositions[Counter];
        TableElement.Position = new Vector2(40 + (GridPosition.column * 160), 40 + (GridPosition.row * 160)) * ScaleValues[TableScale];
    }
    private void SetScale() 
    { 
        TableElementContainer.CustomMinimumSize = new Vector2(2950, 1660) * ScaleValues[TableScale];

        int Counter = 0;

        foreach (Panel TableElement in TableElementContainer.GetChildren()) {
            SetPanelScale(TableElement, Counter);
            Counter++;
        }
    }
    private void ReturnToHome() => GetParent<Hud>().AnimatePages(this, GetNode<HomePage>("../HomePage"));
}
