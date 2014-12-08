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
    IceQueenMelee,
    Carrot,
    Flamethrower,
    Knife,
    Cane,
    Molotov
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
    public string HitSoundClip;
    public string SwingSoundClip;


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
                Damage = 1f;
                HitSoundClip = "Footsteps_Heavy_Snow";
                
                SwingSoundClip = "";
                break;
            case WeaponType.Cane:
                BaseDurability = 30;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 100f;
                CanBreak = true;
                Damage = 4f;

                HitSoundClip = "Club";
                SwingSoundClip = "";
                break;
            case WeaponType.Stick:
                BaseDurability = 10;
                Class = WeaponClass.Melee;
                Range = 1.7f;
                Cooldown = 0.2f;
                Knockback = 150f;
                CanBreak = true;
                Damage = 3f;
               
                HitSoundClip = "Club";
                SwingSoundClip = "";
                break;
            case WeaponType.Knife:
                BaseDurability = 20;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 100f;
                CanBreak = true;
                Damage = 6f;

                HitSoundClip = "Club";
                SwingSoundClip = "";
                break;
            case WeaponType.Molotov:
                BaseDurability = 20;
                Class = WeaponClass.Throw;
                Range = 10f;
                Cooldown = 0.5f;
                ProjectileType = ProjectileType.Molotov;
                Knockback = 100f;
                CanBreak = true;
                Damage = 1f;
                HitSoundClip = "Club";

                SwingSoundClip = "";
                break;
            case WeaponType.SnowmanMelee:
                BaseDurability = -1;
                Class = WeaponClass.Melee;
                Range = 1.5f;
                Cooldown = 0.2f;
                Knockback = 150f;
                CanBreak = false;
                Damage = 5f;
                 HitSoundClip = "Club";
                SwingSoundClip = "";
               
                break;
            case WeaponType.IceQueenMelee:
                BaseDurability = -1;
                Class = WeaponClass.Melee;
                Range = 2f;
                Cooldown = 0.2f;
                Knockback = 150f;
                CanBreak = false;
                Damage = 7f;
                HitSoundClip = "Club";
                SwingSoundClip = "";

                break;
            case WeaponType.Carrot:
                BaseDurability = -1;
                Class = WeaponClass.Throw;
                Range = 11f;
                Cooldown = 0.2f;
                ProjectileType = ProjectileType.Carrot;
                Knockback = 100f;
                CanBreak = false;
                Damage = 5f;
                
                HitSoundClip = "Club";
                SwingSoundClip = "";
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
               
                HitSoundClip = "Flame_Stereo";
                SwingSoundClip = "Flame_Stereo";
                break;
        }

        Durability = BaseDurability;
    }
}
