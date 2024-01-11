using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class WeaponData
    {

        [field: SerializeField][JsonProperty] public float Damage { get; set; }
        [JsonProperty] public float ProjectileSpeed { get; set; }
        [JsonProperty] public float Range { get; set; }
        [JsonProperty] public float FireRate { get; set; }
        [JsonProperty] public float ReloadTime { get; set; }
        [JsonProperty] public int MaxAmmo { get; set; }
        [JsonProperty] public float[] LaunchAngles { get; set; } = new float[] { 0.0f };
        [JsonProperty] public float AngleDeviation { get; set; }

        [JsonProperty] public float ShakeIntensity { get; set; }
        [JsonProperty] public float ShakeFrequency { get; set; }
        [JsonProperty] public float ShakeDuration { get; set; }
    }


    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Weapon : Module
    {
        public const string WEAPON_DATA_PATH = "GameData/Weapons";

        public float DefaultFireRate { get; set; }
        public int CurrentAmmo { get; protected set; }
        protected float LastShootTime { get; set; }

        public WeaponData Data { get; protected set; }

        public Weapon()
        {
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_Weapon");
        }

        public bool CanShoot()
        {
            float timeSinceLastShot = (Time.time - LastShootTime);
            float cooldownTime = 1.0f / Data.FireRate;
            return timeSinceLastShot >= cooldownTime;
        }

        public Projectile ShootProjectile(Projectile projectile)
        {
            projectile.Damage = Data.Damage;
            projectile.Speed = Data.ProjectileSpeed;
            projectile.Range = Data.Range;

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
            Data.Damage *= multiplier;
        }
    }
}
