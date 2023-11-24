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
            Damage = 15.0f;
            ProjectileSpeed = 12.0f;
            Range = 15.0f;

            FireRate = 10.0f;
            DefaultFireRate = FireRate;
            ReloadTime = 2.0f;
            MaxAmmo = 30;
            CurrentAmmo = MaxAmmo;
        }

    }
}
