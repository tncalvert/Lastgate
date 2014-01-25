using UnityEngine;
using System.Collections;

public class Armor : GameItem {

    public Armor()
    {
        ArmorValue = 1;
    }

    public uint ArmorValue { get; set; }
}
