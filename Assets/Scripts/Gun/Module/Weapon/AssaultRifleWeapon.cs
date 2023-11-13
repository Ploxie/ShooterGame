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
            Damage = 15.0f;
            ProjectileSpeed = 12.0f;
            Range = 15.0f;

            FireRate = 10.0f;
            ReloadTime = 2.0f;
            MaxAmmo = 30;
            CurrentAmmo = MaxAmmo;
        }

    }
}
