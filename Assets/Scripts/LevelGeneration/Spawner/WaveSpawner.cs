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
    [SerializeField] public List<GameObject> spawnPoints;
    [SerializeField] public List<GameObject> doors;

    private double waveStart;

    public Wave[] Waves;
    private Player player;

    public int CurrentWaveIndex = 0;
    private bool readyToCountDown;

    private List<Enemy> spawnedEnemies;

    private void Awake()
    {
        spawnedEnemies = new();
    }


    private void Start()
    {
        waveStart = Utils.GetUnixMillis();
        readyToCountDown = true;

        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[i].EnemiesLeft = Waves[i].Enemies.Length;
            Debug.Log(Waves[i].EnemiesLeft);
        }


    }

    private void Update()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
            return;
        }

        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i].Health.IsDead)
            {
                Waves[CurrentWaveIndex].EnemiesLeft--;
                spawnedEnemies.Remove(spawnedEnemies[i]);
            }
        }

        if (player.inWaveRoom == true)
        {
            if (Waves[CurrentWaveIndex].EnemiesLeft == 0)
            {
                readyToCountDown = true;
                waveStart = Utils.GetUnixMillis();
                CurrentWaveIndex++;
            }

            if (CurrentWaveIndex >= Waves.Length)
            {
                foreach (GameObject doors in doors)
                {
                    Door door = doors.GetComponentInChildren<Door>();
                    door.OpenDoor();
                }
                player.inWaveRoom = false;
                Debug.Log("GG you clear all waves");
                return;
            }

            if (spawnedEnemies.Count <= 0)
            {
                if (Utils.GetUnixMillis() - waveStart > 5000)
                {
                    readyToCountDown = false;
                    countdown = Waves[CurrentWaveIndex].TimeToNextEnemy;
                    StartCoroutine(SpawnWave());
                }
            }


        }
    }

    private IEnumerator SpawnWave()
    {
        if (CurrentWaveIndex < Waves.Length)
        {

            for (int i = 0; i < Waves[CurrentWaveIndex].Enemies.Length; i++)
            {
                var spawnPoint = spawnPoints[(int)(UnityEngine.Random.value * spawnPoints.Count)];
                Enemy enemy = Instantiate(Waves[CurrentWaveIndex].Enemies[i]);


                enemy.GetComponent<NavMeshAgent>().enabled = false;
                enemy.transform.position = spawnPoint.transform.position;
                enemy.GetComponent<NavMeshAgent>().enabled = true;

                var idleState = enemy.StateMachine.GetState() as Idle;
                if (idleState != null)
                {
                    idleState.detectionRange = 10000.0f;
                }

                spawnedEnemies.Add(enemy);
                Debug.Log("Spawning enemy: " + spawnPoint.transform.position);
                yield return new WaitForSeconds(Waves[CurrentWaveIndex].TimeToNextEnemy + 0.5f);
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