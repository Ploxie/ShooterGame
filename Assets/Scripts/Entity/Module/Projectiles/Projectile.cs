using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public float Damage { get; set; }
        public float Speed { get; set; }
        public float Range { get; set; }
        public Vector3 Direction { get; set; }
        public List<StatusEffect> StatusEffects { get; set; }     
        public ProjectileEffect ProjectileEffect { get; set; }
        public HashSet<Collider> HitColliders { get; set; }
        protected Rigidbody RigidBody { get; set; }
        protected Collider Collider { get; private set; }
        public Vector3 StartPosition { get; set; }


        protected virtual void Awake()
        {
            StatusEffects = new List<StatusEffect>();
            HitColliders = new HashSet<Collider>();
            RigidBody = GetComponent<Rigidbody>();
            Collider = GetComponent<Collider>();

            StartPosition = transform.position;
        }

        protected virtual void Update()
        {
            transform.position += Direction * Speed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(90.0f, Vector3.Cross(Direction, Vector3.up)) * Quaternion.LookRotation(Direction, Vector3.up);

            if (Vector3.Distance(StartPosition, transform.position) >= Range)
            {
                OnEndOfRange();
            }
        }

        protected virtual void OnEndOfRange()
        {
            Destroy(gameObject);
        }

        protected virtual void OnWallCollision(Collider collider)
        {
            Destroy(gameObject);
        }


        protected virtual void OnCharacterCollision(Character character)
        {
            character.OnBulletCollision(this);            
            Destroy(gameObject);
        }

        protected void OnTriggerEnter(Collider collider)
        {
            if (HitColliders.Contains(collider)) // We don't want to be able to be hit by the same bullet twice
                return;

            if (collider.TryGetComponent(out Character character) && !character.CompareTag(tag))
            {
                OnCharacterCollision(character);
                HitColliders.Add(collider);
            }

            if (collider.CompareTag("Wall"))
            {
                OnWallCollision(null);
                return;
            }
        }

    }
}
