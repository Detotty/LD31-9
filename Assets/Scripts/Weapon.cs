using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum WeaponType
{
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

    public float Range;

    public Weapon(WeaponType type)
    {
        Type = type;

        switch (type)
        {
            case WeaponType.Stick:
                Class = WeaponClass.Melee;
                Range = 2f;
                break;
        }
    }
}
