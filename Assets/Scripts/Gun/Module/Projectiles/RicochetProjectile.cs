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
    public class RicochetData
    {
        public int MaxBounceCount;
    }

    public class RicochetProjectile : Projectile
    {
        public RicochetData Data;
        public int Bounces { get; set; }

        public RicochetProjectile()
        {
            Data = JsonConvert.DeserializeObject<RicochetData>(File.ReadAllText($"{PROJECTILE_DATA_PATH}/Ricochet.json"));
        }


        protected override void OnWallCollision(Collision collision)
        {
            if(Bounces >= Data.MaxBounceCount)
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
