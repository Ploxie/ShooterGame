using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class SniperWeapon : Weapon
    {
        public SniperWeapon()
        {
            Name = "Sniper";
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Sniper");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_65Sniper");
            Description = "The sniper rifle packs a powerful punch from a long distance, but is lacking in the fire rate department.";

            Damage = 50.0f;
            ProjectileSpeed = 30.0f;
            Range = 30.0f;

            FireRate = 0.85f;
            DefaultFireRate = FireRate;
            ReloadTime = 5.0f;
            MaxAmmo = 5;
            CurrentAmmo = MaxAmmo;

            ShakeIntensity = 3.0f;
            ShakeFrequency = 0.5f;
            ShakeDuration = 0.2f;
        }

    }
}
