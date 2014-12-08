using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponType
{
    Snowball,
    Stick,
    SnowmanMelee,
    Carrot,
    Flamethrower
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
    public bool CanBreak;
    public int Durability;
    public int BaseDurability;

    public float Range;

    public Weapon(WeaponType type)
    {
        Type = type;

        switch (type)
        {
            case WeaponType.Snowball:
                BaseDurability = -1;
                Class= WeaponClass.Throw;
                Range = 10f;
                Cooldown = 0.2f;
                ProjectileType = ProjectileType.Snowball;
                Knockback = 100f;
                CanBreak = false;
                Damage = 2f;
                break;
            case WeaponType.Stick:
                BaseDurability = 20;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 150f;
                CanBreak = true;
                Damage = 4f;
                Durability = BaseDurability;
                break;
            case WeaponType.SnowmanMelee:
                BaseDurability = -1;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 150f;
                CanBreak = false;
                Damage = 5f;
                Durability = BaseDurability;
                break;
            case WeaponType.Carrot:
                BaseDurability = -1;
                Class = WeaponClass.Throw;
                Range = 10f;
                Cooldown = 0.2f;
                ProjectileType = ProjectileType.Carrot;
                Knockback = 100f;
                CanBreak = false;
                Damage = 5f;
                break;
            case WeaponType.Flamethrower:
                BaseDurability = 100;
                Class = WeaponClass.Use;
                Range = 10f;
                Cooldown = 0.2f;
                ProjectileType = ProjectileType.Carrot;
                Knockback = 0f;
                CanBreak = true;
                Damage = 0.1f;
                break;
        }

        Durability = BaseDurability;
    }
}
