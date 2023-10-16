
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthBar))]
public abstract class Enemy : Living
{
    [SerializeField] private CartridgePickup cartridgePickup; //assign in code?

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
        SpawnCartridgePickup();
        StateMachine.SetState(typeof(Death));
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.TakeDamage(damage);
    }

    private void SpawnCartridgePickup()
    {
        CartridgePickup temp = Instantiate(cartridgePickup);
    }
}

