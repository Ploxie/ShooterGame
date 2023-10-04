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

    private ModuleHolder weaponModules;
    private ModuleHolder effectModules;
    private ModuleHolder bulletModules;

    [SerializeField] GameEvent OnHealthPackPickUpEvent;

    public override void Awake()
    {
        moduleController = GunVisual.GetComponent<ModuleController>();

        weaponModules = new ModuleHolder();
        effectModules = new ModuleHolder();
        bulletModules = new ModuleHolder();

        weaponModules.Insert(ModuleGenerator.CreateWeaponModule<PistolModule>());
        Module pistol = weaponModules.Peek();
        moduleController.LoadModule(ModuleType.WeaponModule, pistol, weaponModules);

        base.Awake();
    }

    public void PickupModule(ModuleType moduleType, Module module)
    {
        switch (moduleType)
        {
            case ModuleType.WeaponModule:
                weaponModules.Insert(module);
                break;
            case ModuleType.EffectModule:
                effectModules.Insert(module);
                break;
            case ModuleType.BulletModule:
                bulletModules.Insert(module);
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Module weaponModule = weaponModules.Peek();
            if (weaponModule == null)
                weaponModules.Insert(ModuleGenerator.CreateWeaponModule<PistolModule>());
            weaponModule = weaponModules.Cycle();
            moduleController.LoadModule(ModuleType.WeaponModule, weaponModule, weaponModules);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
            moduleController.LoadModule(ModuleType.EffectModule, effectModules.Cycle(), effectModules);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            moduleController.LoadModule(ModuleType.BulletModule, bulletModules.Cycle(), bulletModules);

        base.Update();
    }
}
