using UnityEngine;
using System.Collections;

public class Spell {

    public Spell()
    {
        Cost = 0;
        Damage = 0;
        Name = "Generic Spell";
        Description = "Generic Spell";
        SpellRange = SpellRange.Touch;
        Type = "Spell";
    }

    public uint Cost {get; set;}
    public uint Damage { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public SpellRange SpellRange { get; set; }
    public string Type { get; set; }
}

public enum SpellRange
{
    Touch,
    Ranged
}
