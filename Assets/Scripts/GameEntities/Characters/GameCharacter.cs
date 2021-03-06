﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Master class representing the highest level
/// of character
/// All other classes for characters should be derived from this
/// </summary>
public class GameCharacter : MonoBehaviour {

    // Stats
    public uint Strength { get; set;}
    public uint Agility { get; set; }
    public uint Constitution { get; set; }
    public uint Intelligence { get; set; }

    // Attributes
    public uint Level { get; set; }
    public uint Experience { get; set; }
    public uint ExperienceToNextLevel { get; set; }
	public bool isEnemy { get; set; }

    // Calculated Stats
    public uint BaseHealth { get; set; }
    public uint MaxHealth { get; set; }
    public uint Health { get; set; }
    public uint BaseActionPoints { get; set; }
    public uint MaxActionPoints { get; set; }
    public uint ActionPoints { get; set; }
    public uint ArmorPoints { get; set; }
    public float BaseDamage { get; set; }
    public uint ItemLimit { get; set; }

	//lol
	public uint Kills { get; set; }

    // Items
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }

    public List<GameItem> Inventory { get; set; }

    // Misc.
    public string Type { get; set; }

    public void init()
    {
        // Defaults
        Strength = 8;
        Agility = 8;
        Constitution = 8;
        Intelligence = 8;

        Level = 1;
        Experience = 0;
        ExperienceToNextLevel = 1000;

		isEnemy = false;

        BaseHealth = 25;
        MaxHealth = 25;
        Health = 25;
        BaseActionPoints = 25;
        MaxActionPoints = 25;
        ActionPoints = 25;
        ArmorPoints = 10;
        BaseDamage = 5;
        ItemLimit = 30;

        Weapon = null;
        Armor = null;

        Inventory = new List<GameItem>();

        Type = "GameCharacter";

		// lol
		Kills = 0;

        updateStats();
    }


    // Calculate the calculated stats based on the other stats
    public void updateStats()
    {
        float healthperc = Health / BaseHealth;
        float apperc = ActionPoints / BaseActionPoints;
        uint maxhdiff = MaxHealth - BaseHealth;
        uint maxapdiff = MaxActionPoints - BaseActionPoints;

        BaseHealth = (Constitution * 3) + (2 * Level);
        Health = (uint)(BaseHealth * healthperc);
        MaxHealth = BaseHealth + maxhdiff;
        BaseActionPoints = (Intelligence * 3) + (2 * Level);
        ActionPoints = (uint)(BaseActionPoints * apperc);
        MaxActionPoints = BaseActionPoints + maxapdiff;

        ArmorPoints = 10 + (Armor != null ? Armor.ArmorValue : 0);
        BaseDamage = (Strength + (Agility / 4)) / 2;
        ItemLimit = System.Math.Min(50, Strength * 5);
    }

    //**********************
    // Actions
    //**********************
    public void Attack()
    {
        // TODO: Calculate using weapon
		Collider2D[] hits = Physics2D.OverlapCircleAll (new Vector2(transform.position.x, transform.position.y), 0.2f);
		foreach (Collider2D hit in hits) {
			GameCharacter victim = hit.gameObject.GetComponent<GameCharacter>();

			if (victim == this) continue; // Don't attack yourself!
			
            if ((Type == "Mage" || Type == "Dwarf") && (victim.Type == "Mage" || victim.Type == "Dwarf") /* TODO: add PvP flag */)
                continue;

			Debug.Log ("Hit! from " + this.Type);


            uint damage = (uint)(BaseDamage + (Weapon != null ? Weapon.Damage : 0));
			bool dead = victim.RemoveHealth(damage); //TODO: Refine damage calculation

			if (dead) {
				Kills++;
			}
		}
    }

    //**********************
    // Health
    //**********************
    public void AddHealth(uint amount)
    {
        Health = System.Math.Min(MaxHealth, Health + amount);
    }

    public bool RemoveHealth(uint amount)
    {
		int newHealth = (int)Health - (int)amount;
		Health = (uint)System.Math.Max (newHealth, 0);
        if (Health <= 0)
        {
            // DIE!!
			GUI.Label(new Rect((float)Screen.width / 1.75f, (float)Screen.height / 1.75f, 200, 100), "YOU DIED (FIX THIS)");

			Network.Destroy(this.gameObject);
			return true;
        }
		return false;
    }

    //**********************
    // Action Points
    //**********************
    public void AddActionPoints(uint amount)
    {
        ActionPoints = System.Math.Min(MaxActionPoints, ActionPoints + amount);
    }

    public void RemoveActionPoints(uint amount)
    {
        ActionPoints = System.Math.Max(0, ActionPoints - amount);
    }

    //**********************
    // Level System
    //**********************
    public void AddExperience(uint xp)
    {
        Experience += xp;

        if (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level += 1;
        ExperienceToNextLevel += 1000;
        // Do some level up management stuff??
    }

    //**********************
    // Weapon
    //**********************
    public float GetCalculatedDamage()
    {
        float modifiedDamage = 0;
        if (Weapon.WeaponType == WeaponType.Heavy)
        {
            modifiedDamage += Weapon.Damage * (Strength / 2);
        }
        else if(Weapon.WeaponType == WeaponType.Light)
        {
            modifiedDamage += Weapon.Damage * (Strength / 4) * (Agility / 3);
        }

        return BaseDamage + modifiedDamage;
    }

    public void SetWeapon(Weapon newWeapon)
    {
        if (!IsItemInInventory((GameItem)newWeapon))
        {
            return;
        }

        Weapon oldWeapon = Weapon;
        Weapon = newWeapon;

        RemoveItemFromInventory((GameItem)newWeapon);
        AddItemToInventory((GameItem)oldWeapon);
    }

    //**********************
    // Armor
    //**********************

    public void SetArmor(Armor newArmor)
    {
        if (!IsItemInInventory((GameItem)newArmor))
        {
            return;
        }

        Armor oldArmor = Armor;
        Armor = newArmor;

        RemoveItemFromInventory((GameItem)newArmor);
        AddItemToInventory((GameItem)oldArmor);
    }

    //**********************
    // Inventory Management
    //**********************
    public bool AddItemToInventory(GameItem item)
    {
        if (Inventory.Count >= ItemLimit)
        {
            return false;
        }

        Inventory.Add(item);
        return true;
    }

    public bool RemoveItemFromInventory(GameItem item)
    {
        if (!IsItemInInventory(item))
        {
            return false;
        }

        Inventory.Remove(item);
        return true;
    }

    public bool IsItemInInventory(GameItem item)
    {
        return Inventory.Contains(item);
    }

    //**********************
    // Network
    //**********************
    [RPC]
    void UpdateHealthAndAP(uint health, uint ap)
    {
        Health = health;
        ActionPoints = ap;
    }

    [RPC]
    void UpdateAllHealthAndAP(uint health, uint mhealth, uint bhealth, uint ap, uint map, uint bap)
    {
        BaseHealth = bhealth;
        MaxHealth = mhealth;
        Health = health;
        BaseActionPoints = bap;
        MaxActionPoints = map;
        ActionPoints = ap;
    }

    [RPC]
    void UpdateBaseStats(uint str, uint agil, uint con, uint intel)
    {
        Strength = str;
        Agility = agil;
        Constitution = con;
        Intelligence = intel;
    }

    [RPC]
    void UpdateXP(uint lvl, uint xp, uint xptnl)
    {
        Level = lvl;
        Experience = xp;
        ExperienceToNextLevel = xptnl;
    }

    [RPC]
    void UpdateMiscStats(uint armorP, uint baseDam, uint itemL)
    {
        ArmorPoints = armorP;
        BaseDamage = baseDam;
        ItemLimit = itemL;
    }

    [RPC]
    void UpdateWeaponAndArmor(string w, string a)
    {
        // Find a good way to do this
    }

    [RPC]
    void UpdateInventory(string inv)
    {
        // Find a good way to do this
    }

    [RPC]
    void UpdateType(string type)
    {
        Type = type;
    }

}
