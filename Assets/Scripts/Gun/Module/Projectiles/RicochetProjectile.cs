using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class RicochetProjectile : Projectile
    {

        public int BounceCount { get; set; } = 4;
        public int Bounces { get; set; }

        protected override void OnWallCollision(Collision collision)
        {
            if(Bounces >= BounceCount)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 reflectedDirection = Vector3.Reflect(Direction, collision.GetContact(0).normal);
            Direction = reflectedDirection.normalized;            
            Bounces++;
            StartPosition = RigidBody.position;
        }
    }
}
