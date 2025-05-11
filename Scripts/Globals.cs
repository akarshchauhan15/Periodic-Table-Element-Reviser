using Godot;
using Godot.Collections;
using System.Linq;
public partial class Element : GodotObject
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public int AtomicNumber {  get; set; }
    public float AtomicMass { get; set; }

    public Element(string name, string symbol, int atomicNumber, float atomicMass)
    {
        Name = name;
        Symbol = symbol;
        AtomicNumber = atomicNumber;
        AtomicMass = atomicMass;
    }
    public static Dictionary<long, string> OptionValues = new Dictionary<long, string>
        {
        {0, "Element Name"},
        {1, "Symbol"},
        {2, "Atomic Number"},
        {3, "Atomic Mass"}
        };
}
public class Elements
{
    public static Element Hydrogen = new Element("Hydrogen", "H", 1, 1);
    public static Element Helium = new Element("Helium", "He", 2, 4);
    public static Element Lithium = new Element("Lithium", "Li", 3, 7);
    public static Element Berylium = new Element("Berylium", "Be", 4, 9);
    public static Element Boron = new Element("Boron", "B", 5, 11);

    public static Array<Element> ElementList =
        [
            Hydrogen,
            Helium,
            Lithium,
            Berylium,
            Boron,
        ];
}
public partial class ElementCollection : GodotObject
{
    public static string DisplayName {  get; set; }
    public static int[] ElementsPresent { get; set; }

    public ElementCollection(string displayName, int[] elementsPresent) 
    {
        DisplayName = displayName;
        ElementsPresent = elementsPresent;
    }
    public ElementCollection(string displayName, int StartRange, int EndRange)
    {
        DisplayName = displayName;

        for (int i = StartRange; i <= EndRange; i++) 
            ElementsPresent.Append<int>(i);
    }
    public static Array<Element> GetElementsList()
    {
        Array<Element> CurrentList = new Array<Element>();
        foreach (int Index in ElementsPresent)
            CurrentList.Add(Elements.ElementList[Index]);

        return CurrentList;
    }
}
public class ElementCollections
{
    public static Dictionary<string, ElementCollection> FromBeginning = new Dictionary<string, ElementCollection>
    {
        {"First 10", new ElementCollection("First 10", 1, 10)}
    };
}