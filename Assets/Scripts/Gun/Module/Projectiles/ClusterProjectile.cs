using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entity
{
    public class ClusterData
    {
        public int ProjectileCount;
    }

    public class ClusterProjectile : Projectile
    {
        public ClusterData Data;

        private bool HasExploded;

        public ClusterProjectile()
        {
            Data = JsonConvert.DeserializeObject<ClusterData>(File.ReadAllText($"{PROJECTILE_DATA_PATH}/Cluster.json"));
        }

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

            float placementInterval = 360.0f / Data.ProjectileCount;
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
