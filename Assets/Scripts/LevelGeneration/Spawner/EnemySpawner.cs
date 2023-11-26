using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Entity.Enemy[] Enemies;

        public bool ContinousSpawns;
        public int SpawnCooldown;
        public int MaxActiveEnemies;
        private double lastSpawn;
        private double activeEnemies;
        [SerializeField] public bool SpawnSpecialEnemy = false;
        [SerializeField] public bool RandomEnemyWeakness = true;
        [SerializeField] private SpecialWeakness weakness;

        [field: SerializeField] private bool Active { get; set; } = false;

        private void Start()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            lastSpawn = Utils.GetUnixMillis();
            if (Active)
                StartCoroutine(DelayedSpawn());
        }

        protected void Update()
        {
            if (Active && ContinousSpawns && Utils.GetUnixMillis() - lastSpawn >= SpawnCooldown && activeEnemies < MaxActiveEnemies)
            {
                SpawnRandomEnemy();
                activeEnemies++;
                lastSpawn = Utils.GetUnixMillis();
            }
        }

        private IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(1.0f);
            SpawnRandomEnemy();
        }

        public void SpawnRandomEnemy()
        {
            if (Enemies?.Length <= 0)
                return;

            if (activeEnemies <= MaxActiveEnemies)
            {
                Entity.Enemy enemyToSpawn = Enemies[Random.Range(0, Enemies.Length)];
                Enemy enemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
                if (SpawnSpecialEnemy)
                {
                    if (RandomEnemyWeakness)
                    {
                        enemy.SetSpecial((SpecialWeakness)(Random.value * 5));
                    }
                    else
                    {
                        enemy.SetSpecial(weakness);
                    }
                }


            }
        }

        public void Activate()
        {
            Active = true;
        }
    }
}