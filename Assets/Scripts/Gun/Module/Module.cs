using System;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    [Serializable]
    public abstract class Module
    {

        // TODO: Add name for UI 
        public GameObject DropPrefab { get; protected set; }
        public string Name { get; protected set; } = "Undefined Name";

        public Sprite Icon { get; protected set; }

        public string Description { get; protected set; }

        public Module()
        {
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Default");
        }

        public static Module CreateRandomWeaponModule()
        {
            Module[] modules = new Module[]
            {
                new SniperWeapon(),
                new SMGWeapon(),
                new ShotgunWeapon(),
                new PistolWeapon(),
                new AssaultRifleWeapon()
            };

            return modules[UnityEngine.Random.Range(0, modules.Length)];
        }

        public static Module CreateRandomProjectileEffectModule()
        {
            Module[] modules = new Module[]
            {
                new BlackHoleEffect(),
                new ClusterEffect(),
                new CrystalEffect(),
                new ExplosionEffect(),
                new PiercingEffect(),
                new RicochetEffect()
            };

            return modules[UnityEngine.Random.Range(0, modules.Length)];
        }

        public static StatusEffect CreateRandomStatusEffectModule()
        {
            StatusEffect[] modules = new StatusEffect[]
            {
                new DebilitationEffect(),
                new IceEffect(),
                new RadiationEffect(),
                new StunEffect(),
                new DamageReceivedEffect()
            };

            return modules[UnityEngine.Random.Range(0, modules.Length)];
        }

        public static Module CreateRandomModule()
        {
            Module[] modules = new Module[] 
            {
                new SniperWeapon(),
                new SMGWeapon(),
                new ShotgunWeapon(),
                new PistolWeapon(),
                new AssaultRifleWeapon(),
                new BlackHoleEffect(),
                new ClusterEffect(),
                new CrystalEffect(),
                new ExplosionEffect(),
                new PiercingEffect(),
                new RicochetEffect(),
                new DebilitationEffect(),
                new IceEffect(),
                new RadiationEffect(),
                new StunEffect(),
                new DamageReceivedEffect()
            };

            return modules[UnityEngine.Random.Range(0, modules.Length)];
        }
        
    }
}
