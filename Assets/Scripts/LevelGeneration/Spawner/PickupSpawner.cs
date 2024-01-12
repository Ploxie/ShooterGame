using Assets.Scripts.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class PickupSpawner : MonoBehaviour
    {
        private enum CartridgeType
        {
            Pistol,
            Shotgun,
            Sniper,
            Assault_Rifle,
            SMG,

            //Effect
            Debilitation,
            Ice,
            Radiation,
            Slug,
            Stun,

            //Bullet
            Black_Hole,
            Cluster,
            Crystal,
            Explosive,
            Piercing,
            Ricochet,
        }

        [field: SerializeField] private CartridgeType Type { get; set; }
        public Module SpawnedModule;

        private void Start()
        {
            Module module = Type switch
            {
                CartridgeType.Pistol => new PistolWeapon(),
                CartridgeType.SMG => new SMGWeapon(),
                CartridgeType.Shotgun => new ShotgunWeapon(),
                CartridgeType.Sniper => new SniperWeapon(),
                CartridgeType.Assault_Rifle => new AssaultRifleWeapon(),
                CartridgeType.Debilitation => new DebilitationEffect(),
                CartridgeType.Ice => new IceEffect(),
                CartridgeType.Radiation => new RadiationEffect(),
                CartridgeType.Slug => new DamageReceivedEffect(),
                CartridgeType.Stun => new StunEffect(),
                CartridgeType.Black_Hole => new BlackHoleEffect(),
                CartridgeType.Cluster => new ClusterEffect(),
                CartridgeType.Crystal => new CrystalEffect(),
                CartridgeType.Explosive => new ExplosionEffect(),
                CartridgeType.Piercing => new PiercingEffect(),
                CartridgeType.Ricochet => new RicochetEffect(),
                _ => throw new NotImplementedException("Module not implemented"),
            };

            GameObject prefab = Instantiate(module.DropPrefab, transform.position, Quaternion.identity);
            CartridgePickup pickup = prefab.AddComponent<CartridgePickup>();
            pickup.Module = module;

            SpawnedModule = pickup.Module;
            PopUpConsumable popUp = prefab.AddComponent<PopUpConsumable>();
            popUp.CartridgePickup = pickup;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 1, 0.5f));
        }
    }
}