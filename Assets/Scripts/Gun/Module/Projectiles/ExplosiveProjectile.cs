using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class ExplosionData
    {
        public int ExplosionRange;
    }

    public class ExplosiveProjectile : Projectile
    {
        public ExplosionData Data;

        public ExplosiveProjectile()
        {
            Data = JsonConvert.DeserializeObject<ExplosionData>(File.ReadAllText($"{PROJECTILE_DATA_PATH}/Explosive.json"));
        }

        protected override void OnWallCollision(Collision collision)
        {
            Explode();
            base.OnWallCollision(collision);
        }

        protected override void OnCharacterCollision(Character character)
        {
            Explode();
            base.OnCharacterCollision(character);
        }

        protected override void OnEndOfRange()
        {
            Explode();
            base.OnEndOfRange();
        }

        private void Explode()
        {
            GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefabs/VFX/ExplosionRound"));
            explosion.transform.position = transform.position;
            Collider[] colliders = Physics.OverlapSphere(transform.position, Data.ExplosionRange);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Character character) && !character.CompareTag(tag))
                {
                    character.OnHit(Damage, ProjectileEffect, StatusEffects.ToArray());
                }
            }
        }
    }
}
