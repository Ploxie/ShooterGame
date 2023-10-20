using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class CrystalProjectile : Projectile
    {

        private bool HasExploded;

        protected override void OnWallCollision(Collider collider)
        {
            base.OnWallCollision(collider);
            Explode();
        }

        protected override void OnCharacterCollision(Character character)
        {           
            base.OnCharacterCollision(character);
            Explode();
        }

        protected override void OnEndOfRange()
        {
            base.OnEndOfRange();
            Explode();
        }

        private void Explode()
        {
            if (HasExploded)
                return;

            {
                CrystalProjectile projectile = ProjectileEffect.CreateProjectile(transform.position - (Speed * Time.deltaTime * Direction)) as CrystalProjectile;
                projectile.Direction = Quaternion.AngleAxis(90.0f, Vector3.up) * Direction;
                projectile.Damage = Damage;
                projectile.Speed = Speed;
                projectile.Range = Range;
                projectile.HasExploded = true;
                projectile.StartPosition = transform.position;
                projectile.HitCollisions = HitCollisions;


                if (StatusEffects != null)
                    StatusEffects.ForEach((e) => projectile.StatusEffects.Add(e.Copy()));
            }

            {
                CrystalProjectile projectile = ProjectileEffect.CreateProjectile(transform.position) as CrystalProjectile;
                projectile.Direction = Quaternion.AngleAxis(-90.0f, Vector3.up) * Direction;
                projectile.Damage = Damage;
                projectile.Speed = Speed;
                projectile.Range = Range;
                projectile.HasExploded = true;
                projectile.StartPosition = transform.position;
                projectile.HitCollisions = HitCollisions;


                if (StatusEffects != null)
                    StatusEffects.ForEach((e) => projectile.StatusEffects.Add(e.Copy()));
            }
        }
    }
}
