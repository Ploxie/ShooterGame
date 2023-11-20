using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public abstract class Weapon : Module
    {

        [field: SerializeField] public float Damage { get; protected set; }
        public float ProjectileSpeed { get; protected set; }
        public float Range { get; protected set; }
        public float FireRate { get; protected set; }
        public float ReloadTime { get; protected set; }
        public int MaxAmmo { get; protected set; }
        public int CurrentAmmo { get; protected set; }
        public float[] LaunchAngles { get; set; } = new float[] { 0.0f };
        public float AngleDeviation { get; set; }

        protected float LastShootTime { get; set; }

        public bool CanShoot()
        {
            float timeSinceLastShot = (Time.time - LastShootTime);
            float cooldownTime = 1.0f / FireRate;
            return timeSinceLastShot >= cooldownTime;
        }

        public Projectile ShootProjectile(Projectile projectile)
        {
            projectile.Damage = Damage;
            projectile.Speed = ProjectileSpeed;
            projectile.Range = Range;

            LastShootTime = Time.time;

            //CurrentAmmo--;
            //if (CurrentAmmo <= 0)
            //{
            //    LastShootTime = Time.time + ReloadTime;
            //    CurrentAmmo = MaxAmmo;
            //}

            return projectile;
        }
        public void MultiplyDamage(float multiplier)
        {
            Damage *= multiplier;
        }
    }
}
