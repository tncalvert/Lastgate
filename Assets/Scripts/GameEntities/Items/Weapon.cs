using UnityEngine;
using System.Collections;

public class Weapon : GameItem {

    void Awake()
    {
        init();
        Damage = 2;
        WeaponType = WeaponType.Light;
    }

    public uint Damage { get; set; }
    public WeaponType WeaponType { get; set; }
    
}

public enum WeaponType
{
    Heavy,
    Light
}
