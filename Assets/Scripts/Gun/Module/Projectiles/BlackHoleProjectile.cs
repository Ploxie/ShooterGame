using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entity
{
    public class BlackHoleProjectile : Projectile
    {
        public const float AttractRange = 10;
        public const float RangeIncrements = 2;
        public const float Duration = 1.0f;
        public const float TickCount = 1.0f; // How many times the black hole will attract per second
        public const float PullStrength = 100;
        public const float PullStrengthIncrements = 50;

        protected override void OnWallCollision(Collision collision)
        {
            SpawnBlackHole();
            Destroy(gameObject);
        }

        protected override void OnCharacterCollision(Character character)
        {
            character.OnHit(Damage, StatusEffects.ToArray());
            SpawnBlackHole();
            Destroy(gameObject);
        }

        protected override void OnEndOfRange()
        {
            SpawnBlackHole();
            Destroy(gameObject);
        }

        private void SpawnBlackHole()
        {
            BlackHole blackHole = new GameObject().AddComponent<BlackHole>();
            blackHole.transform.position = transform.position;
            blackHole.tag = tag;
        }

        private IEnumerator Attract()
        {
            Speed = 0.0f;
            for(float time = 0.0f; time < Duration; time += 1.0f / TickCount)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, AttractRange);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent(out Character character) && !character.CompareTag(tag))
                    {
                        Vector3 direction = (transform.position - character.transform.position);                       
                        float strength = (direction.magnitude / AttractRange);
                        direction.y = 0.0f;
                        character.Rigidbody.velocity = direction * 2.5f;//.AddRelativeForce(direction / direction.magnitude);
                    }
                }
                Debug.Log("PULLING");
                yield return new WaitForSeconds(1.0f / TickCount);
            }

            Debug.Log("DESTROY");
            Destroy(gameObject);
        }

    }
}
