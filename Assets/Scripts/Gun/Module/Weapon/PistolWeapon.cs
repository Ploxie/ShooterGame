﻿using System;
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
            Name = "Pistol";
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Pistol");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_50Pistol");
            Description = "The pistol is a reliable weapon in a pinch, but not the thing to take along if you expect any serious challenge.";

            Damage = 15.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 5.0f;
            DefaultFireRate = FireRate;
            ReloadTime = 1.0f;
            MaxAmmo = 10;
            CurrentAmmo = MaxAmmo;

            

            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickup_Pistol");            
        }

    }
}
