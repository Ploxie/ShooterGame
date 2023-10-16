using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    BoltAction,
    SMG,
    Automatic,
    Shotgun
}

public abstract class WeaponModule : Module2
{
    public WeaponType TypeOfWeapon;

    protected WeaponModule()
    {

    }

    public abstract WeaponData GetData();
}
