using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity
{
    public class SMGWeapon : Weapon
    {
        public SMGWeapon()
        {
            Damage = 1.0f;
            ProjectileSpeed = 10.0f;
            Range = 10.0f;

            FireRate = 20.0f;
            ReloadTime = 2.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            LaunchAngles = new float[] { 0, 22.5f, -22.5f, 11.25f, -11.25f };

            AngleDeviation = 10.0f;            
        }
    }
}
