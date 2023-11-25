
using Assets.Scripts.Entity;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [field: SerializeField] public float AttackRange { get; private set; } = 3f;
    [field: SerializeField] public float JumpRange { get; private set; } = 10f;
    [field: SerializeField] public float Damage { get; private set; } = 1.0f;
    [field: SerializeField] public float JumpDamage { get; private set; } = 2.0f;

    [SerializeField] private Hitbox meleeDamageHitBox;

    [SerializeField] private Hitbox jumpDamageHitBox;

    [SerializeField] private GameObject visualCracks;
    private GameObject slash;

    [SerializeField] public DissolveEffect DE;

    private Assets.Scripts.Entity.StatusEffect Effect { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Agent.speed = MovementSpeed;
        DE.Effect.Stop();
        meleeDamageHitBox.Damage = Damage;
        meleeDamageHitBox.Effect = Effect;
        jumpDamageHitBox.Damage = JumpDamage;
        jumpDamageHitBox.Effect = Effect;
        Effect = Module.CreateRandomStatusEffectModule();
        slash = Resources.Load<GameObject>("Prefabs/VFX/Slash");

        meleeDamageHitBox.Effect = Effect;
        jumpDamageHitBox.Effect = Effect;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        //PlaySound("deathmelee");
        SpawnCartridgePickup(Effect);
        DE.enabled = true;
        DE.Effect.Play();
        StateMachine.SetState(typeof(MeleeDeath));
        
    }

    public void ToggleMeleeDamage(int value)
    {
        if (value != 0)
        {
            meleeDamageHitBox.gameObject.SetActive(true);
            GameObject temp = Instantiate(slash, transform);
        }
        else
        {
            meleeDamageHitBox.gameObject.SetActive(false);
        }
    }


    public void ToggleJumpDamage(int value)
    {

        if (value != 0)
        {
            jumpDamageHitBox.gameObject.SetActive(true);
            PlaceEffect();
            //PlaySound("attackgruntsmelee");
            //PlaySound("slammattackmelee");
            AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.ScreamMelee, this.transform.position);
            AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.slamsGround, this.transform.position);
        }
        else
        {
            jumpDamageHitBox.gameObject.SetActive(false);
        }
    }


    public void PlaceEffect()
    {
        GameObject temp = Instantiate(visualCracks);
        temp.transform.position = transform.position;
        temp.transform.Rotate(0, Random.Range(0, 360f), 0);
    }

    protected override void ModifyDamage(float multiplier)
    {
        base.ModifyDamage(multiplier);
        meleeDamageHitBox.Damage *= multiplier;
        jumpDamageHitBox.Damage *= multiplier;
    }

}
    //EnemyManager enemyManager;
    //ScoreManager scoreManager;
    //// Start is called before the first frame update
    //Animator animator;
    //NavMeshAgent agent;
    //[SerializeField]
    //Hitbox jumpDamageHitBox;
    //[SerializeField]
    //Hitbox meleeDamageHitBox;

    //[SerializeField]
    //float jumpSpeedModifier = 7;
    //float walkSpeed = 3.5f;

    //[SerializeField]
    //float detectionRange = 10;
    //[SerializeField]
    //static float staggerDurationMax = 0.5f;
    //float staggerDuration = staggerDurationMax;
    //[SerializeField]
    //float meleeRange = 5;
    //[SerializeField]
    //float jumpAttackRange = 10;

    //private bool canInflictMeleeDamage = false;
    //private bool canInflictJumpDamage = false;

    //float distance;
    //bool isWalking = false;
    //bool isDead = false;

    //[SerializeField]
    //GameObject visualCracks;
    //[SerializeField]
    //Player player;

    //public ModuleID EffectModuleID;
    //public StatusEffectData effectStats;

    //public EffectModule effect;

    //private EnemyHealthBar healthBar;

    //private float deathTimer = 0;

    //[SerializeField] CartridgePickup cartridgeDrop;

    //[SerializeField]
    //State state;

    //enum State
    //{
    //    Idle,
    //    Move,
    //    Jump,
    //    Attack,
    //    Die,
    //    Staggered
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
    //    //loop through hitboxes, set effect


    //    player = FindObjectOfType<Player>();
    //    animator = GetComponent<Animator>();
    //    agent = GetComponent<NavMeshAgent>();
    //    scoreManager = FindFirstObjectByType<ScoreManager>();
    //    jumpDamageHitBox.gameObject.SetActive(false);
    //    meleeDamageHitBox.gameObject.SetActive(false);
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

    //// Update is called once per frame
    //public override void Update()
    //{
    //    base.Update();

    //    agent.speed = MovementSpeed;

    //    distance = Vector3.Distance(transform.position, player.transform.position);

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
    //        if (deathTimer > 10)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    //protected override void OnDeath()
    //{
    //    scoreManager.UpdateText(100); // Eventify
    //    animator.SetTrigger("Die");
    //    agent.isStopped = true;
    //    agent.angularSpeed = 0;
    //    isWalking = false;
    //    state = State.Die;
    //    isDead = true;
    //    animator.SetBool("IsDead", isDead);
    //    canInflictMeleeDamage = false;
    //    canInflictJumpDamage = false;

    //    gameObject.GetComponent<CapsuleCollider>().enabled = false;

    //    if (effect != null)
    //    {
    //        CartridgePickup cartridgeDropInstance = Instantiate(cartridgeDrop, transform.position, Quaternion.identity);
    //        cartridgeDropInstance.Assign(ModuleType.EffectModule, EffectModuleID);
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
    //        case State.Jump:
    //            break;
    //        case State.Attack:
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
    //        default:
    //            break;
    //    }
    //}

    //private void HandleMovement()
    //{
    //    float distance = Vector3.Distance(transform.position, player.transform.position);

    //    if (distance < detectionRange)
    //    {
    //        //Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit hitInfo);

    //        if (distance <= meleeRange)
    //        {
    //            isWalking = false;

    //            if (UnityEngine.Random.Range(1, 100) % 2 == 1)
    //                animator.SetTrigger("Punch");
    //            else
    //                animator.SetTrigger("Swipe");

    //            state = State.Attack;
    //        }
    //        else if (distance <= jumpAttackRange && distance > jumpAttackRange / 2/* && hitInfo.transform.CompareTag("Player")*/)
    //        {
    //            agent.isStopped = false;
    //            isWalking = false;
    //            animator.SetTrigger("JumpAttack");
    //            state = State.Jump;
    //            agent.speed = jumpSpeedModifier;
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

    //public void PlaceEffect()
    //{
    //    GameObject temp = Instantiate(visualCracks);
    //    temp.transform.position = transform.position;
    //    temp.transform.Rotate(0, UnityEngine.Random.Range(0, 360f), 0);
    //}





    //public override void TakeDamage(float damage)
    //{
    //    base.TakeDamage(damage);
    //    healthBar.TakeDamage(damage);
    //}



