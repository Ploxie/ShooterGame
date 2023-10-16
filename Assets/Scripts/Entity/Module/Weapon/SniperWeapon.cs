using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity
{
    public class SniperWeapon : Weapon
    {
        public SniperWeapon()
        {
            Damage = 50.0f;
            ProjectileSpeed = 20.0f;
            Range = 20.0f;

            FireRate = 0.5f;
            ReloadTime = 5.0f;
            MaxAmmo = 5;

            CurrentAmmo = MaxAmmo;
        }

    }
}
