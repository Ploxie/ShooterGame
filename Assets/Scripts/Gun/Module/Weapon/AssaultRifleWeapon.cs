using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class AssaultRifleWeapon : Weapon
    {

        public AssaultRifleWeapon()
        {
            Name = "Assault Rifle";
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_51AssaultRifle");
            Description = "A perfect balance of speed and damage, the assault rifle can adapt to any situation.";
            Damage = 10.0f;
            ProjectileSpeed = 24.0f;
            Range = 30.0f; 

            FireRate = 10.0f;
            DefaultFireRate = FireRate;
            ReloadTime = 2.0f;
            MaxAmmo = 30;
            CurrentAmmo = MaxAmmo;

            ShakeIntensity = 3.0f;
            ShakeFrequency = 0.5f;
            ShakeDuration = 0.2f;
        }

    }
}
