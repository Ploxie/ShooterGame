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

        private void Start()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            lastSpawn = Utils.GetUnixMillis();
            StartCoroutine(DelayedSpawn());
        }

        protected void Update()
        {
            if (ContinousSpawns && Utils.GetUnixMillis() - lastSpawn >= SpawnCooldown && activeEnemies < MaxActiveEnemies)
            {
                SpawnRandomEnemy();
                activeEnemies++;
                lastSpawn = Utils.GetUnixMillis();
            }
        }

        private IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(10.0f);
            SpawnRandomEnemy();
        }

        public void SpawnRandomEnemy()
        {
            if (Enemies?.Length <= 0)
                return;

            Entity.Enemy enemyToSpawn = Enemies[Random.Range(0, Enemies.Length)];
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}