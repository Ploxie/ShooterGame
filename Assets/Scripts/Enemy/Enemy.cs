using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;


[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthBar))]
public abstract class Enemy : Living
{
    [SerializeField] private CartridgePickup cartridgeDrop; //assign in code?

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Player Player { get; private set; }
    private EnemyHealthBar healthBar;

    [SerializeField] public StateMachine StateMachine = new StateMachine();



    public override void Awake()
    {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();
        Player = FindObjectOfType<Player>();
        Animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        StateMachine.Init(this);
    }

    public override void Start()
    {
        base.Start();
        //EnemyManager.Instance.RegisterEnemy(this);
    }

    public override void Update()
    {
        base.Update();
        StateMachine.Update();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.TakeDamage(damage);
    }
}

