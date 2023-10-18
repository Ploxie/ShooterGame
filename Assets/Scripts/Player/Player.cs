using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Living
{
    public GameObject GunVisual;

    private ModuleController moduleController;

    //private ModuleHolder weaponModules;
    //private ModuleHolder effectModules;
    //private ModuleHolder bulletModules;

    //GameEvent OnHealthPackPickUpEvent;

    public override void Awake()
    {
        moduleController = GunVisual.GetComponent<ModuleController>();

        //weaponModules = new ModuleHolder();
        //effectModules = new ModuleHolder();
        //bulletModules = new ModuleHolder();

        //weaponModules.Insert(ModuleGenerator.CreateWeaponModule<PistolModule>());
        //Module2 pistol = weaponModules.Peek();
        //moduleController.LoadModule(ModuleType.WeaponModule, pistol);

        base.Awake();
    }

    public void PickupModule(ModuleType moduleType, Module2 module)
    {
        switch (moduleType)
        {
            case ModuleType.WeaponModule:
                //weaponModules.Insert(module);
                moduleController.LoadModule(ModuleType.WeaponModule, module);
                break;
            case ModuleType.EffectModule:
                //effectModules.Insert(module);
                moduleController.LoadModule(ModuleType.EffectModule, module);
                break;
            case ModuleType.BulletModule:
                //bulletModules.Insert(module);
                moduleController.LoadModule(ModuleType.BulletModule, module);
                break;
        }
    }

    public void OnHealthPackPickUp(Component sender, object data)
    {
        if (data is int) Health += (int)data;
    }

    protected override void OnDeath()
    {
        SceneManager.LoadScene("GameOver");
        base.OnDeath();
    }

    public override void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Module2 weaponModule = weaponModules.Peek();
            if (weaponModule == null)
                weaponModules.Insert(ModuleGenerator.CreateWeaponModule<PistolModule>());
            weaponModule = weaponModules.Cycle();
            moduleController.LoadModule(ModuleType.WeaponModule, weaponModule);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
            moduleController.LoadModule(ModuleType.EffectModule, effectModules.Cycle());

        if (Input.GetKeyDown(KeyCode.Alpha3))
            moduleController.LoadModule(ModuleType.BulletModule, bulletModules.Cycle());

        base.Update();*/
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        //EventManager.TriggerPlayerHealthChanged(Health);
        //EventManager.Instance.TriggerEvent(new PlayerHealthChangeEvent(Health));
    }
}
