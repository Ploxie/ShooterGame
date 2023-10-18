using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Entity.Enemy[] Enemies;

        private void Start()
        {
            Random.InitState(System.DateTime.Now.Millisecond);

            StartCoroutine(DelayedSpawn());
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