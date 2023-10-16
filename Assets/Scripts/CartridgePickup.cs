using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgePickup : MonoBehaviour
{
    public ModuleType type;
    public ModuleID id;

    public Module Module;

    public void Assign(ModuleType type, ModuleID id)
    {
        this.type = type;
        this.id = id;
    }

    public void ApplyModuleTo(Gun gun)
    {
        if (gun == null)
            return;

        if (Module is Assets.Scripts.Entity.StatusEffect statusEffect) //TODO: Remove namespace
        {
            gun.StatusEffect = statusEffect;
        }
        else if (Module is ProjectileEffect projectileEffect)
        {
            gun.ProjectileEffect = projectileEffect;
        }
        else if (Module is Weapon weaponModule)
        {
            gun.Weapon = weaponModule;
        }

        Destroy(gameObject);
    }
}
