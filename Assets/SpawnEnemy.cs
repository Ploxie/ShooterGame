using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private int numberOfSpawns;

    private void Start()
    {
        SpawnRandomEnemy();
    }

    public void SpawnRandomEnemy()
    {
        if (Enemies?.Length <= 0)
            return;

        Instantiate(Enemies[Random.Range(0, Enemies.Length)], transform.position, Quaternion.identity);
    }
}
