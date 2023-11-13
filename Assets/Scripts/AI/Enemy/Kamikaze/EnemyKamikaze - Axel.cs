
using Assets.Scripts.Entity;
using UnityEngine;
using UnityEngine.AI;


public class EnemyKamikaze : Enemy
{

    
    
    
    [field: SerializeField] public float DiveRange { get; private set; } = 3f;
    [field: SerializeField] public float Damage { get; set; } = 10.0f;

    [SerializeField] public Hitbox ExplosionDamageHitBox; // TODO: Create an explosion instance? 
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject healthBarVisual;
    [SerializeField] private DissolveEffect DE;

    public bool HasRoared { get; private set; } = false;

    private Assets.Scripts.Entity.StatusEffect StatusEffect { get; set; }


    protected override void Awake()
    {
        base.Awake();
        DE.Effect.Stop();
        Agent.speed = CurrentMovementSpeed;
        StatusEffect = Module.CreateRandomStatusEffectModule();

        ExplosionDamageHitBox.Damage = Damage;
        ExplosionDamageHitBox.Effect = StatusEffect;
    }
    public void Explode()
    {
        
        ExplosionDamageHitBox.gameObject.SetActive(true);
        explosionVFX.SetActive(true);
        healthBarVisual.SetActive(false);
        
        model.SetActive(false);
        Agent.isStopped = true;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        SpawnCartridgePickup(StatusEffect);
        StateMachine.SetState(typeof(KamikazeDeath));
        DE.enabled = true;
        DE.Effect.Play();
    }

    private void Jank()
    {
        StateMachine.SetState(typeof(KamikazeDeath));
    }
    private void EndRoar()
    {
        HasRoared = true;
    }
}
//EnemyManager enemyManager;
//ScoreManager scoreManager;

//Animator animator;
//NavMeshAgent agent;
//[SerializeField]
//Hitbox explosionDamageHitBox;

//[SerializeField]
//float detectionRange = 50;
//[SerializeField]
//static float staggerDurationMax = 0.5f;
//float staggerDuration = staggerDurationMax;

//[SerializeField]
//float diveRange = 4;

//float distance;
//bool isWalking = false;
//bool isDead = false;
//bool playerDetected = false;

//[SerializeField]
//Player player;

//[SerializeField]
//GameObject model;

//public ModuleID EffectModuleID;
//StatusEffectData effectStats;

//public EffectModule effect;

//private EnemyHealthBar healthBar;

//private float deathTimer = 0;

//[SerializeField] CartridgePickup cartridgeDrop;

//[SerializeField] GameObject healthBarObject;


//[SerializeField]
//State state;
//enum State
//{
//    Idle,
//    Move,
//    Die,
//    Staggered,
//    Roar,
//    Dive
//}

//public override void Start()
//{
//    base.Start();
//    enemyManager = FindObjectOfType<EnemyManager>();
//    enemyManager.RegisterEnemy(this);

//    effect = (EffectModule)ModuleRegistry.CreateModuleByID(EffectModuleID);
//    if (effect != null)
//    {
//        foreach (Transform child in transform)
//        {
//            if (child.TryGetComponent<Hitbox>(out Hitbox hitbox))
//            {
//                hitbox.effect = effect.GetStatusEffect();
//                hitbox.damage = Damage;
//            }
//        }
//    }
//}
//public override void Awake()
//{
//    base.Awake();
//    healthBar = GetComponentInChildren<EnemyHealthBar>();

//    player = FindObjectOfType<Player>();
//    animator = GetComponent<Animator>();
//    agent = GetComponent<NavMeshAgent>();
//    scoreManager = FindFirstObjectByType<ScoreManager>();
//    explosionDamageHitBox.gameObject.SetActive(false);

//    state = State.Idle;
//}

//public void SwapModule(ModuleID id)
//{
//    EffectModuleID = id;
//    effect = (EffectModule)ModuleRegistry.CreateModuleByID(EffectModuleID);
//    foreach (Transform child in transform)
//    {
//        if (child.TryGetComponent<Hitbox>(out Hitbox hitbox))
//        {
//            hitbox.effect = effect.GetStatusEffect();
//            hitbox.damage = Damage;
//        }
//    }
//}

//public override void Update()
//{
//    base.Update();

//    agent.speed = MovementSpeed;

//    if (state != State.Die)
//    {

//        if (distance >= detectionRange)
//        {
//            if (isWalking)
//            {
//                isWalking = false;
//                agent.isStopped = true;
//                animator.SetBool("IsWalking", isWalking);
//            }
//        }
//        else
//        {
//            SetState();

//        }

//    }
//    else
//    {
//        deathTimer += Time.deltaTime;
//        explosionDamageHitBox.transform.localScale += new Vector3(deathTimer, deathTimer, deathTimer) * 0.5f;

//        if (deathTimer > 0.5)
//        {
//            explosionDamageHitBox.gameObject.SetActive(false);
//        }
//        if (deathTimer > 10)
//        {
//            Destroy(gameObject);
//        }
//    }
//}

//private void SetState()
//{

//    switch (state)
//    {
//        case State.Idle:
//            if (isWalking)
//            {
//                isWalking = false;
//                agent.isStopped = true;
//                animator.SetBool("IsWalking", isWalking);
//            }
//            HandleMovement();
//            break;
//        case State.Move:
//            HandleMovement();
//            break;
//        case State.Die:

//            break;
//        case State.Staggered:
//            if (isWalking)
//            {
//                isWalking = false;
//                agent.isStopped = true;
//                animator.SetBool("IsWalking", isWalking);
//            }
//            if (staggerDuration > 0f)
//            {
//                staggerDuration -= Time.deltaTime;
//                if (staggerDuration < 0f)
//                {
//                    staggerDuration = staggerDurationMax;
//                    HandleMovement();
//                }
//            }
//            break;
//        case State.Roar:
//            break;
//        default:
//            break;
//    }
//}

//private void HandleMovement()
//{
//    float distance = Vector3.Distance(transform.position, player.transform.position);

//    if (!playerDetected)
//    {
//        animator.SetTrigger("Roar");
//        state = State.Roar;
//        playerDetected = true;
//        return;
//    }
//    if (distance < detectionRange)
//    {
//        //Debug.Log($"Distance is {distance}, needs to be below {diveRange}");
//        Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit hitInfo);
//        if (distance <= diveRange/* && hitInfo.transform.CompareTag("Player") && state == State.Move*/)
//        {
//            agent.isStopped = false;
//            isWalking = false;
//            animator.SetTrigger("Dive");
//            agent.SetDestination(player.transform.position);
//        }
//        else
//        {

//            agent.isStopped = false;
//            isWalking = true;
//            animator.SetBool("IsWalking", isWalking);

//            agent.SetDestination(player.transform.position);
//            state = State.Move;
//        }
//    }


//}

//protected override void OnDeath()
//{
//    agent.isStopped = true;
//    scoreManager.UpdateText(100);
//    Explode();
//    if (effect != null)
//    {
//        CartridgePickup cartridgeDropInstance = Instantiate(cartridgeDrop, transform.position, Quaternion.identity);
//        cartridgeDropInstance.Assign(ModuleType.EffectModule, EffectModuleID);
//    }
//}

//public void Explode()
//{
//    explosionDamageHitBox.gameObject.SetActive(true);
//    explosionDamageHitBox.Activate();
//    model.gameObject.SetActive(false);
//    agent.isStopped = true;
//    state = State.Die;
//    healthBar.enabled = false;
//    gameObject.GetComponent<CapsuleCollider>().enabled = false;
//    healthBarObject.gameObject.SetActive(false);


//}

//public void EndRoar()
//{
//    state = State.Move;
//}

//public override void TakeDamage(float damage)
//{
//    base.TakeDamage(damage);
//    healthBar.TakeDamage(damage);
//}


