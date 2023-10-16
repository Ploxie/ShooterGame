﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity
{
    public class ShotgunWeapon : Weapon
    {

        public ShotgunWeapon()
        {
            Damage = 1.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 1.0f;
            ReloadTime = 3.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            AngleDeviation = 20.0f;
        }
    }
}
