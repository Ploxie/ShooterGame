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
        public HashSet<Collision> HitCollisions { get; set; }
        protected Rigidbody RigidBody { get; set; }
        protected Collider Collider { get; private set; }
        public Vector3 StartPosition { get; set; }


        protected virtual void Awake()
        {
            StatusEffects = new List<StatusEffect>();
            HitCollisions = new HashSet<Collision>();
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

        protected virtual void OnWallCollision(Collision collision)
        {
            Destroy(gameObject);
        }


        protected virtual void OnCharacterCollision(Character character)
        {
            character.OnHit(Damage, ProjectileEffect, StatusEffects.ToArray());            
            Destroy(gameObject);
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (HitCollisions.Contains(collision)) // We don't want to be able to be hit by the same bullet twice
                return;

            if (collision.collider.gameObject.TryGetComponent(out Character character) && !character.CompareTag(tag))
            {
                OnCharacterCollision(character);
                HitCollisions.Add(collision);
            }

            if (collision.collider.gameObject.CompareTag("Wall") || collision.collider.gameObject.CompareTag("Prop"))
            {
                OnWallCollision(collision);
                return;
            }
        }

    }
}
