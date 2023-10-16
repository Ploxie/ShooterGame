using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

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
            //StateMachine.Init(this);

            Health.OnDamageTaken += TakeDamage;
            Health.OnDeath += OnDeath;
        }

        protected override void Update()
        {
            base.Update();
            //StateMachine.Update();
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        protected void SpawnCartridgePickup(Module module)
        {
            GameObject prefab = Instantiate(module.DropPrefab, transform.position, Quaternion.identity);
            CartridgePickup pickup = prefab.AddComponent<CartridgePickup>();
            {
                pickup.Module = module;
            }
        }

        private void TakeDamage(float damage)
        {
            //healthBar.TakeDamage(damage);
        }
    }

}