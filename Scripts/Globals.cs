using Godot;
using Godot.Collections;
public partial class Element : GodotObject
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public int AtomicNumber {  get; set; }
    public float AtomicMass { get; set; }

    public  Element(string name, string symbol, int atomicNumber, float atomicMass)
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