using Newtonsoft.Json;
using System.Collections;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Entity
{
    public class BlackHoleData
    {
        public float AttractRange;
        public float RangeIncrements;
        public float Duration;
        public float TickCount; // How many times the black hole will attract per second
        public float PullStrength;
        public float PullStrengthIncrements;
    }

    public class BlackHoleProjectile : Projectile
    {
        public BlackHoleData Data;

        public BlackHoleProjectile()
        {
            Data = JsonConvert.DeserializeObject<BlackHoleData>(File.ReadAllText($"{PROJECTILE_DATA_PATH}/BlackHole.json"));
        }

        protected override void OnWallCollision(Collision collision)
        {
            SpawnBlackHole();
            Destroy(gameObject);
        }

        protected override void OnCharacterCollision(Character character)
        {
            character.OnHit(Damage, ProjectileEffect, StatusEffects.ToArray());
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
            GameObject visual = Instantiate(Resources.Load<GameObject>("Prefabs/VFX/BlackHole"), blackHole.transform);
            Vector3 temp = transform.position;
            temp.y = 1.7f;
            visual.transform.position = temp;
        }

        private IEnumerator Attract()
        {
            Speed = 0.0f;
            for(float time = 0.0f; time < Data.Duration; time += 1.0f / Data.TickCount)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, Data.AttractRange);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent(out Character character) && !character.CompareTag(tag))
                    {
                        Vector3 direction = (transform.position - character.transform.position);                       
                        float strength = (direction.magnitude / Data.AttractRange);
                        direction.y = 0.0f;
                        character.Rigidbody.velocity = direction * 2.5f;//.AddRelativeForce(direction / direction.magnitude);
                    }
                }

                yield return new WaitForSeconds(1.0f / Data.TickCount);
            }

            Destroy(gameObject);
        }

    }
}
