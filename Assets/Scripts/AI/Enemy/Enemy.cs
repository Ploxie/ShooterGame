using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthBar), typeof(AudioSource))]
    public abstract class Enemy : Character
    {
        public AudioSource AudioSource { get; private set; }
        public Animator Animator { get; protected set; }
        public NavMeshAgent Agent { get; protected set; }
        public Player Player { get; private set; }

        [HideInInspector]
        public WaveSpawner waveSpawner;

        [SerializeField] public StateMachine StateMachine = new StateMachine();

       
        protected override void Awake()
        {
            base.Awake();
            AudioSource = GetComponent<AudioSource>();
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            waveSpawner = GetComponentInParent<WaveSpawner>();
            StateMachine.Init(this);

            Health.OnDeath += OnDeath;
        }

        protected virtual void Start()
        {
            
        }

        protected override void Update()
        {
            if(Player == null)
                Player = FindObjectOfType<Player>();

            base.Update();
            StateMachine.Update();
        }

        protected virtual void OnDeath()
        {
            //if (Player.inWaveRoom == true)//Ska göra detta bara om man är i rummet med waves
            //{
            //    waveSpawner.Waves[waveSpawner.CurrentWaveIndex].EnemiesLeft--;
            //}
        }

        protected void SpawnCartridgePickup(Module module)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 0.0f;

            GameObject prefab = Instantiate(module.DropPrefab, spawnPosition, Quaternion.identity);
            CartridgePickup pickup = prefab.AddComponent<CartridgePickup>();
            {
                pickup.Module = module;
            }
        }

        public void PlaySound(string key)
        {
            EventManager.GetInstance().TriggerEvent(new AudioEvent(AudioSource, key));
        }
    }
}
