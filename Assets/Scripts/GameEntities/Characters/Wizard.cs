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

    public void LearnSpell(Spell spell)
    {
        if (!KnowSpell(spell))
            return;

        Spells.Add(spell);
    }

    public bool KnowSpell(Spell spell)
    {
        return Spells.Contains(spell);
    }
}
