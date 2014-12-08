using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponType
{
    Snowball,
    Stick
}

public enum WeaponClass
{
    Melee,
    Throw,
    Use
}

public class Weapon 
{
    public WeaponType Type;
    public WeaponClass Class;
    public ProjectileType ProjectileType;
    public float Cooldown;
    public float Knockback;
    public float Damage;
    public int Durability;
    public int BaseDurability;

    public float Range;

    public Weapon(WeaponType type)
    {
        Type = type;

        switch (type)
        {
            case WeaponType.Snowball:
                Class= WeaponClass.Throw;
                Range = 10f;
                Cooldown = 0.2f;
                ProjectileType = ProjectileType.Snowball;
                Knockback = 100f;
                Durability = -1;
                Damage = 2f;
                break;
            case WeaponType.Stick:
                BaseDurability = 20;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 150f;
                Durability = 20;
                Damage = 4f;
                Durability = BaseDurability;
                break;
        }
    }
}
