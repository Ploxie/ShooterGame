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

    public void Awake()
    {
        EffectStats stats = new EffectStats();
        stats.Duration = 10000;
        stats.Interval = 1000;
        weaponModule = ModuleGenerator.CreateWeaponModule<AutomaticModule>();
        effectModule = ModuleGenerator.CreateEffectModule<RadiationModule>(stats);
        bulletModule = ModuleGenerator.CreateBulletModule<CrystalModule>();

        gunVisual = GetComponent<GunVisual>();
        gunVisual.UpdateVisuals(weaponModule);
    }

    public void LoadModule(ModuleType type, Module module)
    {
        //Load logic here
    }

    public void EjectModule(ModuleType type)
    {
        //Ejection logic here
    }

    public void ApplyEffects(Projectile projectile)
    {
        projectile.AddStatusEffect(effectModule.GetStatusEffect());
        projectile.AddBulletEffect(bulletModule.GetBulletEffect());
    }

    public WeaponData GetWeaponData()
    {
        return weaponModule.GetData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
