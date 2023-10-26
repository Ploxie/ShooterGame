using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;

    public Wave[] Waves;
    public Player player;

    public int CurrentWaveIndex = 0;
    private bool readyToCountDown;

    [SerializeField]
    private NavMeshSurface navmesh;//Tas bort sen

    private void Start()
    {
        navmesh.BuildNavMesh();//Tas bort sen
        readyToCountDown = true;

        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[i].EnemiesLeft = Waves[i].Enemies.Length;
        }
    }

    private void Update()
    {
        if (CurrentWaveIndex >= Waves.Length)
        {
            player.inWaveRoom = false;
            Debug.Log("GG you clear all waves");
            return;
        }


        if (readyToCountDown == true)
            countdown -= Time.deltaTime;

        if (countdown < 0)
        {
            readyToCountDown = false;
            countdown = Waves[CurrentWaveIndex].TimeToNextEnemy;
            StartCoroutine(SpawnWave());
        }

        if (Waves[CurrentWaveIndex].EnemiesLeft == 0)
        {
            readyToCountDown = true;
            CurrentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (CurrentWaveIndex < Waves.Length)
        {
            for (int i = 0; i < Waves[CurrentWaveIndex].Enemies.Length; i++)
            {
                Enemy enemy = Instantiate(Waves[CurrentWaveIndex].Enemies[i], spawnPoint.transform);

                enemy.transform.SetParent(spawnPoint.transform);

                yield return new WaitForSeconds(Waves[CurrentWaveIndex].TimeToNextEnemy);
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] Enemies;
    public float TimeToNextEnemy;

    public int EnemiesLeft;
}
