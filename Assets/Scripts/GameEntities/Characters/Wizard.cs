using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wizard : GameCharacter
{

    void Awake()
    {
        init();
        Spells = new List<Spell>();
    }

    public List<Spell> Spells;

    public void CastSpell(Spell spell)
    {
        // ????
    }
}
