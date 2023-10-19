﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class SMGWeapon : Weapon
    {
        public SMGWeapon()
        {
            Name = "SMG";
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_SMG");

            Damage = 1.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 20.0f;
            ReloadTime = 2.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            AngleDeviation = 10.0f;            
        }
    }
}