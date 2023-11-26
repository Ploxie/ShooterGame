using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    public class BlackHole : MonoBehaviour
    {
        public float Radius { get; set; } = 7.0f;
        public float PullsPerSecond { get; set; } = 1.0f;
        public float Duration { get; set; } = 0.5f;
        private float AttractCooldown { get; set; }
        private float AliveTimer { get; set; }


        private void Update()
        {          
            if(AttractCooldown <= 0.0f)
            {
                Attract();

                AttractCooldown = PullsPerSecond / Duration;
            }

            AttractCooldown -= Time.deltaTime;
            AliveTimer += Time.deltaTime;

            if (AliveTimer >= Duration)
            {
                Destroy(gameObject);
            }
        }

        private void Attract()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Character character) && !character.CompareTag(tag))
                {
                    Vector3 direction = (transform.position - character.transform.position);
                    float strength = (direction.magnitude / Radius);
                    direction.y = 0.0f;
                    character.Rigidbody.AddForce(direction * 300);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.1f);
            Gizmos.DrawSphere(transform.position, Radius);
        }

    }
}
