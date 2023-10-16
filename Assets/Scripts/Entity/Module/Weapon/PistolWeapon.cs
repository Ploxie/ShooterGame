using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class PistolWeapon : Weapon
    {

        public PistolWeapon()
        {
            Damage = 15.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 5.0f;
            ReloadTime = 1.0f;
            MaxAmmo = 10;
            CurrentAmmo = MaxAmmo;

            

            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickup_Pistol");            
        }

    }
}
