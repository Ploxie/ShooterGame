using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKamikaze : Living
{
    EnemyManager enemyManager;

    Animator animator;
    NavMeshAgent agent;
    [SerializeField]
    Hitbox explosionDamageHitBox;

    [SerializeField]
    float detectionRange = 50;
    [SerializeField]
    static float staggerDurationMax = 0.5f;
    float staggerDuration = staggerDurationMax;

    [SerializeField]
    float diveRange = 4;

    float distance;
    bool isWalking = false;
    bool isDead = false;
    bool playerDetected = false;

    [SerializeField]
    Player player;

    [SerializeField]
    GameObject model;

    EffectStats effectStats;

    public EffectModule effect;

    private EnemyHealthBar healthBar;

    private float deathTimer = 0;

    [SerializeField] CartridgePickup cartridgeDrop;


    [SerializeField]
    State state;
    enum State
    {
        Idle,
        Move,
        Die,
        Staggered,
        Roar,
        Dive
    }

    public override void Start()
    {
        base.Start();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);

        if (effect != null)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<Hitbox>(out Hitbox hitbox))
                {
                    hitbox.effect = effect.GetStatusEffect();
                }
            }
        }
    }
    public override void Awake()
    {
        base.Awake();
        healthBar = FindFirstObjectByType<EnemyHealthBar>();

        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        explosionDamageHitBox.gameObject.SetActive(false);

        state = State.Idle;
    }


    public override void Update()
    {
        base.Update();

        if (state != State.Die)
        {

            if (distance >= detectionRange)
            {
                if (isWalking)
                {
                    isWalking = false;
                    agent.isStopped = true;
                    animator.SetBool("IsWalking", isWalking);
                }
            }
            else
            {
                SetState();

            }

        }
        else
        {
            deathTimer += Time.deltaTime;
            Debug.Log(deathTimer);
            if (deathTimer > 2)
            {
                explosionDamageHitBox.gameObject.SetActive(false);
            }
            if (deathTimer > 10)
            {
                Destroy(gameObject);
            }
        }
    }

    private void SetState()
    {

        switch (state)
        {
            case State.Idle:
                if (isWalking)
                {
                    isWalking = false;
                    agent.isStopped = true;
                    animator.SetBool("IsWalking", isWalking);
                }
                HandleMovement();
                break;
            case State.Move:
                HandleMovement();
                break;
            case State.Die:

                break;
            case State.Staggered:
                if (isWalking)
                {
                    isWalking = false;
                    agent.isStopped = true;
                    animator.SetBool("IsWalking", isWalking);
                }
                if (staggerDuration > 0f)
                {
                    staggerDuration -= Time.deltaTime;
                    if (staggerDuration < 0f)
                    {
                        staggerDuration = staggerDurationMax;
                        HandleMovement();
                    }
                }
                break;
            case State.Roar:
                break;
            default:
                break;
        }
    }

    private void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (!playerDetected)
        {
            animator.SetTrigger("Roar");
            state = State.Roar;
            playerDetected = true;
            return;
        }
        if (distance < detectionRange)
        {
            //Debug.Log($"Distance is {distance}, needs to be below {diveRange}");
            Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit hitInfo);
            if (distance <= diveRange/* && hitInfo.transform.CompareTag("Player") && state == State.Move*/)
            {
                agent.isStopped = false;
                isWalking = false;
                animator.SetTrigger("Dive");
                agent.SetDestination(player.transform.position);
            }
            else
            {

                agent.isStopped = false;
                isWalking = true;
                animator.SetBool("IsWalking", isWalking);

                agent.SetDestination(player.transform.position);
                state = State.Move;
            }
        }


    }

    protected override void OnDeath()
    {
        Explode();
    }

    void Explode()
    {
        explosionDamageHitBox.gameObject.SetActive(true);
        explosionDamageHitBox.Activate();
        model.gameObject.SetActive(false);
        state = State.Die;
        healthBar.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        if (effect != null)
        {
            CartridgePickup cartridgeDropInstance = Instantiate(cartridgeDrop, transform.position, Quaternion.identity);
            cartridgeDropInstance.Assign(ModuleType.EffectModule, effect);
        }
    }

    public void EndRoar()
    {
        state = State.Move;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.TakeDamage(damage);
    }

}
