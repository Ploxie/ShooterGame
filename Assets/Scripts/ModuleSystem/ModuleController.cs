using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleType
{
    WeaponModule,
    EffectModule,
    BulletModule
}

public class ModuleController : MonoBehaviour
{
    private WeaponModule weaponModule;
    private EffectModule effectModule;
    private BulletModule bulletModule;

    private GunVisual gunVisual;

    private GunVisual GunVisual
    {
        get 
        {
            if (gunVisual == null)                
                gunVisual = GetComponent<GunVisual>();
            
            return gunVisual;
        }
    }

    public void LoadModule(ModuleType type, Module module)
    {
        switch (type)
        {
            case ModuleType.WeaponModule:
                weaponModule = (WeaponModule)module;
                GunVisual.UpdateVisuals(weaponModule.TypeOfWeapon);
                break;
            case ModuleType.EffectModule:
                effectModule = (EffectModule)module;
                break;
            case ModuleType.BulletModule:
                bulletModule = (BulletModule)module;
                break;
        }
    }

    public WeaponData GetWeaponData()
    {
        if (weaponModule == null)
            throw new Exception("WeaponModule is null. This should not be possible.");

        return weaponModule.GetData();
    }

    public StatusEffect GetStatusEffect()
    {
        if (effectModule == null)
            return null;

        return effectModule.GetStatusEffect();
    }

    public BulletEffect GetBulletEffect()
    {
        if (bulletModule == null)
            return null;

        return bulletModule.GetBulletEffect();
    }

}
