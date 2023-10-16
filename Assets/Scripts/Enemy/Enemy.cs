using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Scripts.Entity
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthBar))]
    public abstract class Enemy : Character
    {

        public Animator Animator { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Player Player { get; private set; }

        [SerializeField] public StateMachine StateMachine = new StateMachine();
        protected override void Awake()
        {
            base.Awake();
            Agent = GetComponent<NavMeshAgent>();
            Player = FindObjectOfType<Player>();
            Animator = GetComponent<Animator>();
            StateMachine.Init(this);

            Health.OnDeath += OnDeath;
        }

        protected virtual void Start()
        {
            //EnemyManager.Instance.RegisterEnemy(this);
        }

        protected override void Update()
        {
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
    }
}
