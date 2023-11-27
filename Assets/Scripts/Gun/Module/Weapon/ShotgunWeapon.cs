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
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Shotgun");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_87Shotgun");
            Description = "Firing a wave of bullets, the shotgun is unparalled when facing many foes.";

            Damage = 5.0f;
            ProjectileSpeed = 20.0f;
            Range = 30.0f;

            FireRate = 1.0f;
            DefaultFireRate = FireRate;
            ReloadTime = 3.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            LaunchAngles = new float[] { 0, 5.625f, -5.625f, 11.25f, -11.25f };
            AngleDeviation = 10.0f;
        }
    }
}
