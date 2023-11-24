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

        private PowerUpPickUp powerUpPickUp;
        private Material material;



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
            powerUpPickUp = Resources.Load<PowerUpPickUp>("Prefabs/Pickups/PowerUp");
            StateMachine.Init(this);
           

            Health.OnDeath += OnDeath;
        }

        protected virtual void Start()
        {

        }

        protected override void Update()
        {
            if (Player == null)
                Player = FindObjectOfType<Player>();

            base.Update();
            StateMachine.Update();
        }

        protected virtual void OnDeath()
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            if (isSpecial)
            {
                PowerUpPickUp temp = Instantiate(powerUpPickUp);
                temp.transform.position = transform.position;
            }
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
            PopUpConsumable popUp = prefab.AddComponent<PopUpConsumable>();
            popUp.CartridgePickup = pickup;
        }

        public void PlaySound(string key)
        {
            EventManager.GetInstance().TriggerEvent(new AudioEvent(AudioSource, key));
        }

        protected virtual void ModifyDamage(float multiplier)
        {
            Health.Multiply(10);
            transform.localScale *= 1.5f;
                if (material == null)
                    material = GetComponentInChildren<SkinnedMeshRenderer>().material;
            switch (weakness)
            {
                case SpecialWeakness.Dense:
                    material.SetColor("_BaseColour", Color.yellow);
                    break;
                case SpecialWeakness.Analytic:
                    material.SetColor("_BaseColour", Color.cyan);
                    break;
                case SpecialWeakness.Intangible:
                    material.SetColor("_BaseColour", Color.green);
                    break;
                case SpecialWeakness.Porous:
                    material.SetColor("_BaseColour", Color.red);
                    break;
                case SpecialWeakness.Unstable:
                    material.SetColor("_BaseColour", Color.magenta);
                    break;
                case SpecialWeakness.Armored:
                    material.SetColor("_BaseColour", Color.gray);
                    material.SetFloat("_Metallica", 0.75f);
                    break;
            }
        }
        public override void SetSpecial(SpecialWeakness weakness)
        {
            base.SetSpecial(weakness);
            ModifyDamage(1.5f);
        }

    }
}
