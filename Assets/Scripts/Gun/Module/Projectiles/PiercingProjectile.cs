using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class PiercingProjectile : Projectile
    {
        protected override void OnCharacterCollision(Character character)
        {
            character.OnHit(Damage, StatusEffects.ToArray());
        }
        protected override void OnWallCollision(Collision collision)
        {
        }
    }
}
