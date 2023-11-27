using System;
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
            //DropPrefab = Resources.Load<GameObject>("Prefabs/Pickups/Pickup_SMG");
            Icon = Resources.Load<Sprite>("Sprites/Modules/Weapon/t_78SMG");
            Description = "The SMG unleashes a hell of bullets upon your foes, killing them with a thousand cuts.";

            Damage = 5.0f;
            ProjectileSpeed = 20.0f;
            Range = 20.0f;

            FireRate = 20.0f;
            DefaultFireRate = FireRate;
            ReloadTime = 2.0f;
            MaxAmmo = 20;
            CurrentAmmo = MaxAmmo;

            AngleDeviation = 10.0f;            
        }
    }
}
