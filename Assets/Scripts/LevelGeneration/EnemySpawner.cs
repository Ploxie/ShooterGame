using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public enum EnemyType
    {
        Kamikaze,
        Melee,
        Ranged
    }

    [System.Serializable]
    public struct EnemySpawnInfo
    {
        public EnemyType TypeOfEnemy;
        public GameObject EnemyPrefab;
    }

    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnInfo[] Enemies;

        private void Start()
        {
            SpawnRandomEnemy();
        }

        public void SpawnRandomEnemy()
        {
            if (Enemies?.Length <= 0)
                return;

            Random.InitState(System.DateTime.Now.Millisecond);

            EnemySpawnInfo spawnInfo = Enemies[Random.Range(0, Enemies.Length)];
            GameObject enemy = Instantiate(spawnInfo.EnemyPrefab, transform.position, Quaternion.identity);
            ModuleID randomModule;
            switch (spawnInfo.TypeOfEnemy)
            {
                case EnemyType.Kamikaze:
                    EnemyKamikaze kamikazeEnemy = enemy.GetComponent<EnemyKamikaze>();
                    randomModule = (ModuleID)Random.Range(5, 9);
                    kamikazeEnemy.SwapModule(randomModule);
                    break;
                case EnemyType.Melee:
                    EnemyMelee meleeEnemy = enemy.GetComponent<EnemyMelee>();
                    randomModule = (ModuleID)Random.Range(5, 9);
                    meleeEnemy.SwapModule(randomModule);
                    break;
                case EnemyType.Ranged:
                    EnemyRanged rangedEnemy = enemy.GetComponent<EnemyRanged>();
                    ModuleID weaponID = (ModuleID)Random.Range(0, 4);
                    ModuleID effectID = (ModuleID)Random.Range(5, 9);
                    ModuleID bulletID = (ModuleID)Random.Range(10, 15);
                    rangedEnemy.SwapModules(weaponID, effectID, bulletID);
                    break;
            }

            
        }
    }
}