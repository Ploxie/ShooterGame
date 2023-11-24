using Assets.Scripts.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] public List<GameObject> spawnPoints;
    [SerializeField] public List<GameObject> doors;

    public Wave[] Waves;
    private Player player;

    public int CurrentWaveIndex = 0;
    private bool readyToCountDown;


    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[i].EnemiesLeft = Waves[i].Enemies.Length * spawnPoints.Count;
        }

        
    }

    private void Update()
    {
        if(player == null)
        {
            player = FindAnyObjectByType<Player>();
            return;
        }

        if (player.inWaveRoom == true)
        {
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


            if (readyToCountDown == true)
                countdown -= Time.deltaTime;

            if (countdown <= 0)
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
    }

    private IEnumerator SpawnWave()
    {
        if (CurrentWaveIndex < Waves.Length)
        {
            for (int i = 0; i < Waves[CurrentWaveIndex].Enemies.Length; i++)
            {
                var spawnPoint = spawnPoints[(int)(UnityEngine.Random.value * spawnPoints.Count)];
                Enemy enemy = Instantiate(Waves[CurrentWaveIndex].Enemies[i]);
                enemy.transform.position = spawnPoint.transform.position;
                yield return new WaitForSeconds(Waves[CurrentWaveIndex].TimeToNextEnemy + 1.0f);
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