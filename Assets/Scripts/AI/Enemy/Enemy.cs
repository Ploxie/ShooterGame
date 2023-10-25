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

        [SerializeField] public StateMachine StateMachine = new StateMachine();

       
        protected override void Awake()
        {
            base.Awake();
            AudioSource = GetComponent<AudioSource>();
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
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
            StateMachine.SetState(typeof(Death));
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
