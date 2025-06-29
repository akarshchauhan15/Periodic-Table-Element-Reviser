using Godot;
using Godot.Collections;
using System.Collections.Generic;

public enum ElementCategory
{
    AlkaliMetal,
    AlkaliEarthMetal,
    TransitionMetal,
    PostTransitionMetal,
    Metalloid,
    Halogen,
    NonMetal,
    NobleGas,
    Lanthanides,
    Actinides,
    Transactinides
}
public partial class Element : GodotObject
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public int AtomicNumber {  get; set; }
    public float AtomicMass { get; set; }
    public ElementCategory Category { get; set; }

    public Element(string name, string symbol, int atomicNumber, float atomicMass, ElementCategory elementCategory)
    {
        Name = name;
        Symbol = symbol;
        AtomicNumber = atomicNumber;
        AtomicMass = atomicMass;
        Category = elementCategory;
    }
    public static Godot.Collections.Dictionary<long, string> OptionValues = new Godot.Collections.Dictionary<long, string>
        {
        {0, "Element Name"},
        {1, "Symbol"},
        {2, "Atomic Number"},
        {3, "Atomic Mass"},
        };

    public static StringName[] Properties = 
    [
        PropertyName.Name,
        PropertyName.Symbol,
        PropertyName.AtomicNumber,
        PropertyName.AtomicMass
    ];
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

    public ElementCollection(string Name, int[][] rangeArray, int[] additionalElements = null)
    {
        DisplayName = Name;
        foreach(int[] Range in rangeArray)
        {
            if (Range.Length != 2)
                continue;

            for (int i = Range[0]; i <= Range[1]; i++)
                ElementsPresent.Add(i);
        }
        if (additionalElements != null)
        ElementsPresent.AddRange(additionalElements);
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
        new ElementCollection("First 2", [[1,2]]),
        new ElementCollection("First 10", [[1,10]]),
        new ElementCollection("First 18", [[1, 18]]),
        new ElementCollection("First 36", [[1, 36]]),
        new ElementCollection("First 54", [[1, 54]]),
        new ElementCollection("First 86", [[1, 86]]),
        new ElementCollection("To 86 w/o Lanthanides", [[1, 56], [72, 86]]),
        new ElementCollection("All 118", [[1, 118]]),
        new ElementCollection("To 118 w/o f-Block", [[1, 56], [72, 88], [104, 118]])
    };

    public static Array<ElementCollection> Groups = new Array<ElementCollection>
    {
        new ElementCollection("Group 1 (Alkali Metals)", [1, 3, 11, 19, 37, 55, 87]),
        new ElementCollection("Group 2 (Alkali Earth Metals)", [4, 12, 20, 38, 56, 88]),
        new ElementCollection("Group 13", [5, 13, 31, 49, 81, 113]),
        new ElementCollection("Group 14", [6, 14, 32, 50, 82, 114]),
        new ElementCollection("Group 15", [7, 15, 33, 51, 83, 115]),
        new ElementCollection("Group 16 (Chalcogens)", [8, 16, 34, 52, 84, 116]),
        new ElementCollection("Group 17 (Halogens)", [9, 17, 35, 53, 85, 117]),
        new ElementCollection("Group 18 (Noble Gases)", [2, 10, 18, 36, 54, 86, 118])
    };

    public static Array<ElementCollection> Periods = new Array<ElementCollection>
    {
        new ElementCollection("Period 2", [[3, 10]]),
        new ElementCollection("Period 3", [[11, 18]]),
        new ElementCollection("Period 4", [[19, 36]]),
        new ElementCollection("Period 5", [[37, 54]]),
        new ElementCollection("Period 6", [[55, 86]]),
        new ElementCollection("Period 6 w/o Lanthanides", [[55, 56], [72, 86]]),
        new ElementCollection("Period 7", [[87, 118]]),
        new ElementCollection("Period 7 w/o Actinides", [[87, 88], [104, 118]])
    };

    public static Array<ElementCollection> Blocks = new Array<ElementCollection>
    {
        new ElementCollection("s-Block", [1, 2, 3, 4, 11, 12, 19, 20, 37, 38, 55, 56, 87, 88]),
        new ElementCollection("p-Block", [[5, 10], [13, 18], [31, 36], [49, 54], [81, 86]]),
        new ElementCollection("d-Block", [[21, 30], [39, 48], [72, 80]], [57, 89]),
        new ElementCollection("f-Block", [[57, 71], [89, 103]])
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
    public static Element Hydrogen = new Element("Hydrogen", "H", 1, 1.008f, ElementCategory.NonMetal);
    public static Element Helium = new Element("Helium", "He", 2, 4.002f, ElementCategory.NobleGas);
    public static Element Lithium = new Element("Lithium", "Li", 3, 7, ElementCategory.AlkaliMetal);
    public static Element Beryllium = new Element("Beryllium", "Be", 4, 9.01f, ElementCategory.AlkaliEarthMetal);
    public static Element Boron = new Element("Boron", "B", 5, 10.81f, ElementCategory.Metalloid);
    public static Element Carbon = new Element("Carbon", "C", 6, 12.011f, ElementCategory.NonMetal);
    public static Element Nitrogen = new Element("Nitrogen", "N", 7, 14.007f, ElementCategory.NonMetal);
    public static Element Oxygen = new Element("Oxygen", "O", 8, 15.999f, ElementCategory.NonMetal);
    public static Element Fluorine = new Element("Fluorine", "F", 9, 18.998f, ElementCategory.Halogen);
    public static Element Neon = new Element("Neon", "Ne", 10, 20.180f, ElementCategory.NobleGas);
    public static Element Sodium = new Element("Sodium", "Na", 11, 22.989f, ElementCategory.AlkaliMetal);
    public static Element Magnesium = new Element("Magnesium", "Mg", 12, 24.305f, ElementCategory.AlkaliEarthMetal);
    public static Element Aluminium = new Element("Aluminium", "Al", 13, 26.981f, ElementCategory.PostTransitionMetal);
    public static Element Silicon = new Element("Silicon", "Si", 14, 28.085f, ElementCategory.Metalloid);
    public static Element Phosphorus = new Element("Phosphorus", "P", 15, 30.973f, ElementCategory.NonMetal);
    public static Element Sulfur = new Element("Sulphur", "S", 16, 32.07f, ElementCategory.NonMetal);
    public static Element Chlorine = new Element("Chlorine", "Cl", 17, 35.45f, ElementCategory.Halogen);
    public static Element Argon = new Element("Argon", "Ar", 18, 39.45f, ElementCategory.NobleGas);
    public static Element Potassium = new Element("Potassium", "K", 19, 39.098f, ElementCategory.AlkaliMetal);
    public static Element Calcium = new Element("Calcium", "Ca", 20, 40.08f, ElementCategory.AlkaliEarthMetal);
    public static Element Scandium = new Element("Scandium", "Sc", 21, 44.955f, ElementCategory.TransitionMetal);
    public static Element Titanium = new Element("Titanium", "Ti", 22, 47.867f, ElementCategory.TransitionMetal);
    public static Element Vanadium = new Element("Vanadium", "V", 23, 50.941f, ElementCategory.TransitionMetal);
    public static Element Chromium = new Element("Chromium", "Cr", 24, 51.996f, ElementCategory.TransitionMetal);
    public static Element Manganese = new Element("Manganese", "Mn", 25, 54.933f, ElementCategory.TransitionMetal);
    public static Element Iron = new Element("Iron", "Fe", 26, 55.84f, ElementCategory.TransitionMetal);
    public static Element Cobalt = new Element("Cobalt", "Co", 27, 58.933f, ElementCategory.TransitionMetal);
    public static Element Nickel = new Element("Nickel", "Ni", 28, 58.693f, ElementCategory.TransitionMetal);
    public static Element Copper = new Element("Copper", "Cu", 29, 63.55f, ElementCategory.TransitionMetal);
    public static Element Zinc = new Element("Zinc", "Zn", 30, 65.4f, ElementCategory.TransitionMetal);
    public static Element Gallium = new Element("Gallium", "Ga", 31, 69.723f, ElementCategory.PostTransitionMetal);
    public static Element Germanium = new Element("Germanium", "Ge", 32, 72.63f, ElementCategory.Metalloid);
    public static Element Arsenic = new Element("Arsenic", "As", 33, 74.921f, ElementCategory.Metalloid);
    public static Element Selenium = new Element("Selenium", "Se", 34, 78.97f, ElementCategory.NonMetal);
    public static Element Bromine = new Element("Bromine", "Br", 35, 79.90f, ElementCategory.Halogen);
    public static Element Krypton = new Element("Krypton", "Kr", 36, 83.80f, ElementCategory.NobleGas);
    public static Element Rubidium = new Element("Rubidium", "Rb", 37, 85.468f, ElementCategory.AlkaliMetal);
    public static Element Strontium = new Element("Strontium", "Sr", 38, 87.62f, ElementCategory.AlkaliEarthMetal);
    public static Element Yttrium = new Element("Yttrium", "Y", 39, 88.905f, ElementCategory.TransitionMetal);
    public static Element Zirconium = new Element("Zirconium", "Zr", 40, 91.22f, ElementCategory.TransitionMetal);
    public static Element Niobium = new Element("Niobium", "Nb", 41, 92.906f, ElementCategory.TransitionMetal);
    public static Element Molybdenum = new Element("Molybdenum", "Mo", 42, 95.95f, ElementCategory.TransitionMetal);
    public static Element Technetium = new Element("Technetium", "Tc", 43, 96.906f, ElementCategory.TransitionMetal);
    public static Element Ruthenium = new Element("Ruthenium", "Ru", 44, 101.1f, ElementCategory.TransitionMetal);
    public static Element Rhodium = new Element("Rhodium", "Rh", 45, 102.905f, ElementCategory.TransitionMetal);
    public static Element Palladium = new Element("Palladium", "Pd", 46, 106.42f, ElementCategory.TransitionMetal);
    public static Element Silver = new Element("Silver", "Ag", 47, 107.868f, ElementCategory.TransitionMetal);
    public static Element Cadmium = new Element("Cadmium", "Cd", 48, 112.41f, ElementCategory.TransitionMetal);
    public static Element Indium = new Element("Indium", "In", 49, 114.818f, ElementCategory.PostTransitionMetal);
    public static Element Tin = new Element("Tin", "Sn", 50, 118.71f, ElementCategory.PostTransitionMetal);
    public static Element Antimony = new Element("Antimony", "Sb", 51, 121.760f, ElementCategory.Metalloid);
    public static Element Tellurium = new Element("Tellurium", "Te", 52, 127.6f, ElementCategory.Metalloid);
    public static Element Iodine = new Element("Iodine", "I", 53, 126.904f, ElementCategory.Halogen);
    public static Element Xenon = new Element("Xenon", "Xe", 54, 131.29f, ElementCategory.NobleGas);
    public static Element Caesium = new Element("Caesium", "Cs", 55, 132.905f, ElementCategory.AlkaliMetal);
    public static Element Barium = new Element("Barium", "Ba", 56, 137.33f, ElementCategory.AlkaliEarthMetal);
    public static Element Lanthanum = new Element("Lanthanum", "La", 57, 138.905f, ElementCategory.TransitionMetal);
    public static Element Cerium = new Element("Cerium", "Ce", 58, 140.116f, ElementCategory.Lanthanides);
    public static Element Praseodymium = new Element("Praseodymium", "Pr", 59, 140.907f, ElementCategory.Lanthanides);
    public static Element Neodymium = new Element("Neodymium", "Nd", 60, 144.240f, ElementCategory.Lanthanides);
    public static Element Promethium = new Element("Promethium", "Pm", 61, 144.913f, ElementCategory.Lanthanides);
    public static Element Samarium = new Element("Samarium", "Sm", 62, 150.360f, ElementCategory.Lanthanides);
    public static Element Europium = new Element("Europium", "Eu", 63, 151.964f, ElementCategory.Lanthanides);
    public static Element Gadolinium = new Element("Gadolinium", "Gd", 64, 157.200f, ElementCategory.Lanthanides);
    public static Element Terbium = new Element("Terbium", "Tb", 65, 158.925f, ElementCategory.Lanthanides);
    public static Element Dysprosium = new Element("Dysprosium", "Dy", 66, 162.500f, ElementCategory.Lanthanides);
    public static Element Holmium = new Element("Holmium", "Ho", 67, 164.930f, ElementCategory.Lanthanides);
    public static Element Erbium = new Element("Erbium", "Er", 68, 167.260f, ElementCategory.Lanthanides);
    public static Element Thulium = new Element("Thulium", "Tm", 69, 168.934f, ElementCategory.Lanthanides);
    public static Element Ytterbium = new Element("Ytterbium", "Yb", 70, 173.050f, ElementCategory.Lanthanides);
    public static Element Lutetium = new Element("Lutetium", "Lu", 71, 174.967f, ElementCategory.Lanthanides);
    public static Element Hafnium = new Element("Hafnium", "Hf", 72, 178.490f, ElementCategory.TransitionMetal);
    public static Element Tantalum = new Element("Tantalum", "Ta", 73, 180.948f, ElementCategory.TransitionMetal);
    public static Element Tungsten = new Element("Tungsten", "W", 74, 183.840f, ElementCategory.TransitionMetal);
    public static Element Rhenium = new Element("Rhenium", "Re", 75, 186.207f, ElementCategory.TransitionMetal);
    public static Element Osmium = new Element("Osmium", "Os", 76, 190.200f, ElementCategory.TransitionMetal);
    public static Element Iridium = new Element("Iridium", "Ir", 77, 192.220f, ElementCategory.TransitionMetal);
    public static Element Platinum = new Element("Platinum", "Pt", 78, 195.080f, ElementCategory.TransitionMetal);
    public static Element Gold = new Element("Gold", "Au", 79, 196.967f, ElementCategory.TransitionMetal);
    public static Element Mercury = new Element("Mercury", "Hg", 80, 200.590f, ElementCategory.TransitionMetal);
    public static Element Thallium = new Element("Thallium", "Tl", 81, 204.383f, ElementCategory.PostTransitionMetal);
    public static Element Lead = new Element("Lead", "Pb", 82, 207.000f, ElementCategory.PostTransitionMetal);
    public static Element Bismuth = new Element("Bismuth", "Bi", 83, 208.980f, ElementCategory.PostTransitionMetal);
    public static Element Polonium = new Element("Polonium", "Po", 84, 208.982f, ElementCategory.Metalloid);
    public static Element Astatine = new Element("Astatine", "At", 85, 209.987f, ElementCategory.Halogen);
    public static Element Radon = new Element("Radon", "Rn", 86, 222.018f, ElementCategory.NobleGas);
    public static Element Francium = new Element("Francium", "Fr", 87, 223.020f, ElementCategory.AlkaliMetal);
    public static Element Radium = new Element("Radium", "Ra", 88, 226.025f, ElementCategory.AlkaliEarthMetal);
    public static Element Actinium = new Element("Actinium", "Ac", 89, 227.028f, ElementCategory.TransitionMetal);
    public static Element Thorium = new Element("Thorium", "Th", 90, 232.038f, ElementCategory.Actinides);
    public static Element Protactinium = new Element("Protactinium", "Pa", 91, 231.036f, ElementCategory.Actinides);
    public static Element Uranium = new Element("Uranium", "U", 92, 238.029f, ElementCategory.Actinides);
    public static Element Neptunium = new Element("Neptunium", "Np", 93, 237.048f, ElementCategory.Actinides);
    public static Element Plutonium = new Element("Plutonium", "Pu", 94, 244.064f, ElementCategory.Actinides);
    public static Element Americium = new Element("Americium", "Am", 95, 243.061f, ElementCategory.Actinides);
    public static Element Curium = new Element("Curium", "Cm", 96, 247.070f, ElementCategory.Actinides);
    public static Element Berkelium = new Element("Berkelium", "Bk", 97, 247.070f, ElementCategory.Actinides);
    public static Element Californium = new Element("Californium", "Cf", 98, 251.080f, ElementCategory.Actinides);
    public static Element Einsteinium = new Element("Einsteinium", "Es", 99, 252.083f, ElementCategory.Actinides);
    public static Element Fermium = new Element("Fermium", "Fm", 100, 257.095f, ElementCategory.Actinides);
    public static Element Mendelevium = new Element("Mendelevium", "Md", 101, 258.098f, ElementCategory.Actinides);
    public static Element Nobelium = new Element("Nobelium", "No", 102, 259.101f, ElementCategory.Actinides);
    public static Element Lawrencium = new Element("Lawrencium", "Lr", 103, 266.120f, ElementCategory.Actinides);
    public static Element Rutherfordium = new Element("Rutherfordium", "Rf", 104, 267.122f, ElementCategory.Transactinides);
    public static Element Dubnium = new Element("Dubnium", "Db", 105, 268.126f, ElementCategory.Transactinides);
    public static Element Seaborgium = new Element("Seaborgium", "Sg", 106, 269.128f, ElementCategory.Transactinides);
    public static Element Bohrium = new Element("Bohrium", "Bh", 107, 270.133f, ElementCategory.Transactinides);
    public static Element Hassium = new Element("Hassium", "Hs", 108, 269.134f, ElementCategory.Transactinides);
    public static Element Meitnerium = new Element("Meitnerium", "Mt", 109, 277.154f, ElementCategory.Transactinides);
    public static Element Darmstadtium = new Element("Darmstadtium", "Ds", 110, 282.166f, ElementCategory.Transactinides);
    public static Element Roentgenium = new Element("Roentgenium", "Rg", 111, 282.169f, ElementCategory.Transactinides);
    public static Element Copernicium = new Element("Copernicium", "Cn", 112, 286.179f, ElementCategory.Transactinides);
    public static Element Nihonium = new Element("Nihonium", "Nh", 113, 286.182f, ElementCategory.PostTransitionMetal);
    public static Element Flerovium = new Element("Flerovium", "Fl", 114, 290.192f, ElementCategory.PostTransitionMetal);
    public static Element Moscovium = new Element("Moscovium", "Mc", 115, 290.196f, ElementCategory.PostTransitionMetal);
    public static Element Livermorium = new Element("Livermorium", "Lv", 116, 293.205f, ElementCategory.PostTransitionMetal);
    public static Element Tennessine = new Element("Tennessine", "Ts", 117, 294.211f, ElementCategory.Halogen);
    public static Element Oganesson = new Element("Oganesson", "Og", 118, 295.216f, ElementCategory.NobleGas);


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
    Lawrencium,
    Rutherfordium,
    Dubnium,
    Seaborgium,
    Bohrium,
    Hassium,
    Meitnerium,
    Darmstadtium,
    Roentgenium,
    Copernicium,
    Nihonium,
    Flerovium,
    Moscovium,
    Livermorium,
    Tennessine,
    Oganesson
    };
    public static List<(int column, int row)> ElementGridPositions = new()
{
    (0, 0), (17, 0),
    (0, 1), (1, 1), (12, 1), (13, 1), (14, 1), (15, 1), (16, 1), (17, 1),
    (0, 2), (1, 2), (12, 2), (13, 2), (14, 2), (15, 2), (16, 2), (17, 2),
    (0, 3), (1, 3), (2, 3), (3, 3), (4, 3), (5, 3), (6, 3), (7, 3), (8, 3), (9, 3), (10, 3), (11, 3), (12, 3), (13, 3), (14, 3), (15, 3), (16, 3), (17, 3),
    (0, 4), (1, 4), (2, 4), (3, 4), (4, 4), (5, 4), (6, 4), (7, 4), (8, 4), (9, 4), (10, 4), (11, 4), (12, 4), (13, 4), (14, 4), (15, 4), (16, 4), (17, 4),
    (0, 5), (1, 5), (2, 5),
    (2, 8), (3, 8), (4, 8), (5, 8), (6, 8), (7, 8), (8, 8), (9, 8), (10, 8), (11, 8), (12, 8), (13, 8), (14, 8), (15, 8),
    (3, 5), (4, 5), (5, 5), (6, 5), (7, 5), (8, 5), (9, 5), (10, 5), (11, 5), (12, 5), (13, 5), (14, 5), (15, 5), (16, 5), (17, 5),
    (0, 6), (1, 6), (2, 6),
    (2, 9), (3, 9), (4, 9), (5, 9), (6, 9), (7, 9), (8, 9), (9, 9), (10, 9), (11, 9), (12, 9), (13, 9), (14, 9), (15, 9),
    (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6), (10, 6), (11, 6), (12, 6), (13, 6), (14, 6), (15, 6), (16, 6), (17, 6),
};
}