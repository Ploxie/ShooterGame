using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class ExplosiveProjectile : Projectile
    {
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

        }
    }
}
