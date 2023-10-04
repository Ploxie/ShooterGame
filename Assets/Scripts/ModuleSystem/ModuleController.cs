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

    public void LoadModule(ModuleType type, Module module, ModuleHolder inventory)
    {
        switch (type)
        {
            case ModuleType.WeaponModule:
                WeaponModule weaponModule = (WeaponModule)module;
                inventory.Insert(weaponModule);
                GunVisual.UpdateVisuals(weaponModule.TypeOfWeapon);
                break;
            case ModuleType.EffectModule:
                inventory.Insert((EffectModule)module);
                break;
            case ModuleType.BulletModule:
                inventory.Insert((BulletModule)module);
                break;
        }
    }

    public void DecrementDurabilities(ModuleHolder weaponModuleInventory, ModuleHolder effectModuleInventory, ModuleHolder bulletModuleInventory)
    {
        Module weaponModule = weaponModuleInventory.Peek();
        if (weaponModule != null)
            weaponModule.DecrementDurability(1);

        Module effectModule = effectModuleInventory.Peek();
        if (effectModule != null)
            effectModule.DecrementDurability(1);

        Module bulletModule = bulletModuleInventory.Peek();
        if (bulletModule != null)
            bulletModule.DecrementDurability(1);
    }

    public WeaponData GetWeaponData(ModuleHolder weaponModuleInventory)
    {
        WeaponModule weaponModule = (WeaponModule)weaponModuleInventory.Peek();
        if (weaponModule == null)
            throw new Exception("WeaponModule is null. This should not be possible.");

        if (weaponModule.Durability <= 0)
        {
            weaponModuleInventory.Pop();
            LoadModule(ModuleType.WeaponModule, ModuleRegistry.CreateModuleByID(ModuleID.WPN_PISTOL), weaponModuleInventory);
        }
            

        return weaponModule.GetData();
    }

    public StatusEffect GetStatusEffect(ModuleHolder effectModuleInventory)
    {
        EffectModule effectModule = (EffectModule)effectModuleInventory.Peek();
        if (effectModule == null)
            return null;

        if (effectModule.Durability <= 0)
        {
            effectModuleInventory.Pop();
            return null;
        }

        return effectModule.GetStatusEffect();
    }

    public BulletEffect GetBulletEffect(ModuleHolder bulletModuleInventory)
    {
        BulletModule bulletModule = (BulletModule)bulletModuleInventory.Peek();
        if (bulletModule == null)
            return null;

        if (bulletModule.Durability <= 0)
        {
            bulletModuleInventory.Pop();
            return null;
        }

        return bulletModule.GetBulletEffect();
    }

}
