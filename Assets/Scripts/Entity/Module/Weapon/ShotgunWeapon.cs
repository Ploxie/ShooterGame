using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class ShotgunWeapon : Weapon
    {

        public ShotgunWeapon()
        {
            Name = "Shotgun";
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Shotgun");

            Damage = 1.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 1.0f;
            ReloadTime = 3.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            LaunchAngles = new float[] { 0, 5.625f, -5.625f, 11.25f, -11.25f };
            AngleDeviation = 10.0f;
        }
    }
}
