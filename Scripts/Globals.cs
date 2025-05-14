using Godot;
using Godot.Collections;
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
        {3, "Atomic Mass"},
        };
}

public partial class ElementCollection : GodotObject
{
    public string DisplayName {  get; set; }
    public Array<int> ElementsPresent = new Array<int>();

    public ElementCollection(string displayName, int[] elementsPresent)
    {
        DisplayName = displayName;
        ElementsPresent.AddRange(elementsPresent);
    }

    public ElementCollection(string Name, int[][] rangeArray)
    {
        DisplayName = Name;
        foreach(int[] Range in rangeArray)
        {
            if (Range.Length != 2)
                continue;

            for (int i = Range[0]; i <= Range[1]; i++)
                ElementsPresent.Add(i);
        }
    }

    public Array<Element> GetElementsList()
    {
        Array<Element> CurrentList = new Array<Element>();
        foreach (int Index in ElementsPresent)
            CurrentList.Add(Elements.ElementList[Index-1]);

        return CurrentList;
    }
}
public class ElementCollections
{
    public static Array<ElementCollection> FromBeginning = new Array<ElementCollection>
    {
        new ElementCollection("First 10", [[1,10]]),
        new ElementCollection("First 18", [[1, 18]]),
        new ElementCollection("First 36", [[1, 36]]),
        new ElementCollection("First 54", [[1, 54]]),
        new ElementCollection("First 86", [[1, 86]]),
    };

    public static Array<ElementCollection> Groups = new Array<ElementCollection>
    {
        new ElementCollection("Group 2", [[3, 10]]),
        new ElementCollection("Group 3", [[11, 18]])
    };

    public static Array<ElementCollection> Periods = new Array<ElementCollection>
    {
        new ElementCollection("Period A", [[1, 3, 11, 19, 37, 55, 87]])
    };

    public static Array<ElementCollection> Blocks = new Array<ElementCollection>
    {
        new ElementCollection("S Block", [1, 2, 3, 4, 11, 12, 19, 20, 37, 38, 55, 56, 87, 88]),
        new ElementCollection("F Block", [[57, 71], [89, 103]])
    };

    public static Array<ElementCollection> Others = new Array<ElementCollection>
    {
        new ElementCollection("Lanthanides", [[57, 71]]),
        new ElementCollection("Actinides", [[89, 103]])
    };

    public static Array<Array<ElementCollection>> CollectionArray = new Array<Array<ElementCollection>>
    {
        FromBeginning, Groups, Periods, Blocks, Others
    };
}
public class Elements
{
    public static Element Hydrogen = new Element("Hydrogen", "H", 1, 1);
    public static Element Helium = new Element("Helium", "He", 2, 4);
    public static Element Lithium = new Element("Lithium", "Li", 3, 7);
    public static Element Beryllium = new Element("Beryllium", "Be", 4, 9);
    public static Element Boron = new Element("Boron", "B", 5, 11);
    public static Element Carbon = new Element("Carbon", "C", 6, 12);
    public static Element Nitrogen = new Element("Nitrogen", "N", 7, 14);
    public static Element Oxygen = new Element("Oxygen", "O", 8, 16);
    public static Element Fluorine = new Element("Fluorine", "F", 9, 19);
    public static Element Neon = new Element("Neon", "Ne", 10, 20);
    public static Element Sodium = new Element("Sodium", "Na", 11, 23);
    public static Element Magnesium = new Element("Magnesium", "Mg", 12, 24);
    public static Element Aluminium = new Element("Aluminium", "Al", 13, 27);
    public static Element Silicon = new Element("Silicon", "Si", 14, 28);
    public static Element Phosphorus = new Element("Phosphorus", "P", 15, 31);
    public static Element Sulfur = new Element("Sulfur", "S", 16, 32);
    public static Element Chlorine = new Element("Chlorine", "Cl", 17, 35);
    public static Element Argon = new Element("Argon", "Ar", 18, 40);
    public static Element Potassium = new Element("Potassium", "K", 19, 39);
    public static Element Calcium = new Element("Calcium", "Ca", 20, 40);
    public static Element Scandium = new Element("Scandium", "Sc", 21, 45);
    public static Element Titanium = new Element("Titanium", "Ti", 22, 48);
    public static Element Vanadium = new Element("Vanadium", "V", 23, 51);
    public static Element Chromium = new Element("Chromium", "Cr", 24, 52);
    public static Element Manganese = new Element("Manganese", "Mn", 25, 55);
    public static Element Iron = new Element("Iron", "Fe", 26, 56);
    public static Element Cobalt = new Element("Cobalt", "Co", 27, 59);
    public static Element Nickel = new Element("Nickel", "Ni", 28, 59);
    public static Element Copper = new Element("Copper", "Cu", 29, 64);
    public static Element Zinc = new Element("Zinc", "Zn", 30, 65);
    public static Element Gallium = new Element("Gallium", "Ga", 31, 70);
    public static Element Germanium = new Element("Germanium", "Ge", 32, 73);
    public static Element Arsenic = new Element("Arsenic", "As", 33, 75);
    public static Element Selenium = new Element("Selenium", "Se", 34, 79);
    public static Element Bromine = new Element("Bromine", "Br", 35, 80);
    public static Element Krypton = new Element("Krypton", "Kr", 36, 84);
    public static Element Rubidium = new Element("Rubidium", "Rb", 37, 85);
    public static Element Strontium = new Element("Strontium", "Sr", 38, 88);
    public static Element Yttrium = new Element("Yttrium", "Y", 39, 89);
    public static Element Zirconium = new Element("Zirconium", "Zr", 40, 91);
    public static Element Niobium = new Element("Niobium", "Nb", 41, 93);
    public static Element Molybdenum = new Element("Molybdenum", "Mo", 42, 96);
    public static Element Technetium = new Element("Technetium", "Tc", 43, 98);
    public static Element Ruthenium = new Element("Ruthenium", "Ru", 44, 101);
    public static Element Rhodium = new Element("Rhodium", "Rh", 45, 103);
    public static Element Palladium = new Element("Palladium", "Pd", 46, 106);
    public static Element Silver = new Element("Silver", "Ag", 47, 108);
    public static Element Cadmium = new Element("Cadmium", "Cd", 48, 112);
    public static Element Indium = new Element("Indium", "In", 49, 115);
    public static Element Tin = new Element("Tin", "Sn", 50, 119);
    public static Element Antimony = new Element("Antimony", "Sb", 51, 122);
    public static Element Tellurium = new Element("Tellurium", "Te", 52, 128);
    public static Element Iodine = new Element("Iodine", "I", 53, 127);
    public static Element Xenon = new Element("Xenon", "Xe", 54, 131);
    public static Element Caesium = new Element("Caesium", "Cs", 55, 133);
    public static Element Barium = new Element("Barium", "Ba", 56, 137);
    public static Element Lanthanum = new Element("Lanthanum", "La", 57, 139);
    public static Element Cerium = new Element("Cerium", "Ce", 58, 140);
    public static Element Praseodymium = new Element("Praseodymium", "Pr", 59, 141);
    public static Element Neodymium = new Element("Neodymium", "Nd", 60, 144);
    public static Element Promethium = new Element("Promethium", "Pm", 61, 145);
    public static Element Samarium = new Element("Samarium", "Sm", 62, 150);
    public static Element Europium = new Element("Europium", "Eu", 63, 152);
    public static Element Gadolinium = new Element("Gadolinium", "Gd", 64, 157);
    public static Element Terbium = new Element("Terbium", "Tb", 65, 159);
    public static Element Dysprosium = new Element("Dysprosium", "Dy", 66, 163);
    public static Element Holmium = new Element("Holmium", "Ho", 67, 165);
    public static Element Erbium = new Element("Erbium", "Er", 68, 167);
    public static Element Thulium = new Element("Thulium", "Tm", 69, 169);
    public static Element Ytterbium = new Element("Ytterbium", "Yb", 70, 173);
    public static Element Lutetium = new Element("Lutetium", "Lu", 71, 175);
    public static Element Hafnium = new Element("Hafnium", "Hf", 72, 178);
    public static Element Tantalum = new Element("Tantalum", "Ta", 73, 181);
    public static Element Tungsten = new Element("Tungsten", "W", 74, 184);
    public static Element Rhenium = new Element("Rhenium", "Re", 75, 186);
    public static Element Osmium = new Element("Osmium", "Os", 76, 190);
    public static Element Iridium = new Element("Iridium", "Ir", 77, 192);
    public static Element Platinum = new Element("Platinum", "Pt", 78, 195);
    public static Element Gold = new Element("Gold", "Au", 79, 197);
    public static Element Mercury = new Element("Mercury", "Hg", 80, 201);
    public static Element Thallium = new Element("Thallium", "Tl", 81, 204);
    public static Element Lead = new Element("Lead", "Pb", 82, 207);
    public static Element Bismuth = new Element("Bismuth", "Bi", 83, 209);
    public static Element Polonium = new Element("Polonium", "Po", 84, 209);
    public static Element Astatine = new Element("Astatine", "At", 85, 210);
    public static Element Radon = new Element("Radon", "Rn", 86, 222);
    public static Element Francium = new Element("Francium", "Fr", 87, 223);
    public static Element Radium = new Element("Radium", "Ra", 88, 226);
    public static Element Actinium = new Element("Actinium", "Ac", 89, 227);
    public static Element Thorium = new Element("Thorium", "Th", 90, 232);
    public static Element Protactinium = new Element("Protactinium", "Pa", 91, 231);
    public static Element Uranium = new Element("Uranium", "U", 92, 238);
    public static Element Neptunium = new Element("Neptunium", "Np", 93, 237);
    public static Element Plutonium = new Element("Plutonium", "Pu", 94, 244);
    public static Element Americium = new Element("Americium", "Am", 95, 243);
    public static Element Curium = new Element("Curium", "Cm", 96, 247);
    public static Element Berkelium = new Element("Berkelium", "Bk", 97, 247);
    public static Element Californium = new Element("Californium", "Cf", 98, 251);
    public static Element Einsteinium = new Element("Einsteinium", "Es", 99, 252);
    public static Element Fermium = new Element("Fermium", "Fm", 100, 257);
    public static Element Mendelevium = new Element("Mendelevium", "Md", 101, 258);
    public static Element Nobelium = new Element("Nobelium", "No", 102, 259);
    public static Element Lawrencium = new Element("Lawrencium", "Lr", 103, 266);

    public static Array<Element> ElementList = new Array<Element>
    {
    Hydrogen,
    Helium,
    Lithium,
    Beryllium,
    Boron,
    Carbon,
    Nitrogen,
    Oxygen,
    Fluorine,
    Neon,
    Sodium,
    Magnesium,
    Aluminium,
    Silicon,
    Phosphorus,
    Sulfur,
    Chlorine,
    Argon,
    Potassium,
    Calcium,
    Scandium,
    Titanium,
    Vanadium,
    Chromium,
    Manganese,
    Iron,
    Cobalt,
    Nickel,
    Copper,
    Zinc,
    Gallium,
    Germanium,
    Arsenic,
    Selenium,
    Bromine,
    Krypton,
    Rubidium,
    Strontium,
    Yttrium,
    Zirconium,
    Niobium,
    Molybdenum,
    Technetium,
    Ruthenium,
    Rhodium,
    Palladium,
    Silver,
    Cadmium,
    Indium,
    Tin,
    Antimony,
    Tellurium,
    Iodine,
    Xenon,
    Caesium,
    Barium,
    Lanthanum,
    Cerium,
    Praseodymium,
    Neodymium,
    Promethium,
    Samarium,
    Europium,
    Gadolinium,
    Terbium,
    Dysprosium,
    Holmium,
    Erbium,
    Thulium,
    Ytterbium,
    Lutetium,
    Hafnium,
    Tantalum,
    Tungsten,
    Rhenium,
    Osmium,
    Iridium,
    Platinum,
    Gold,
    Mercury,
    Thallium,
    Lead,
    Bismuth,
    Polonium,
    Astatine,
    Radon,
    Francium,
    Radium,
    Actinium,
    Thorium,
    Protactinium,
    Uranium,
    Neptunium,
    Plutonium,
    Americium,
    Curium,
    Berkelium,
    Californium,
    Einsteinium,
    Fermium,
    Mendelevium,
    Nobelium,
    Lawrencium
    };
}