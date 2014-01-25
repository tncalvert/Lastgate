using UnityEngine;
using System.Collections;

public class Armor : GameItem {

    void Awake()
    {
        init();
        ArmorValue = 1;
    }

    public uint ArmorValue { get; set; }
}
