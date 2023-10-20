using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entity
{
    public class ClusterProjectile : Projectile
    {

        public const int DISTANCE_FROM_CENTER = 1;
        public const int PROJECTILE_COUNT = 50;
        public const int PROJECTILE_COUNT_INCREMENT = 1;

        private bool HasExploded;

        protected override void OnWallCollision(Collision collision)
        {
            base.OnWallCollision(collision);
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

            float angleDeviation = Random.Range(0, 360);

            float placementInterval = 360.0f / PROJECTILE_COUNT;
            for (float i = 0; i < 360; i += placementInterval)
            {
                Vector3 direction = Quaternion.AngleAxis(i + angleDeviation, Vector3.up) * Vector3.forward;
                direction.Normalize();

                ClusterProjectile projectile = ProjectileEffect.CreateProjectile(transform.position - (Direction*Speed*Time.deltaTime)) as ClusterProjectile;
                projectile.Direction = direction;
                projectile.Damage = Damage;
                projectile.Speed = Speed;
                projectile.Range = Range;
                projectile.StartPosition = transform.position;
                projectile.HitCollisions = HitCollisions;
                projectile.HasExploded = true;
                projectile.tag = tag;
                

                if (StatusEffects != null)
                    StatusEffects.ForEach((e) => projectile.StatusEffects.Add(e.Copy()));

            }
        }
    }
}
