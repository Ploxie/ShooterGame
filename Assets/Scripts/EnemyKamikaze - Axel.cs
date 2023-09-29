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
    movePlayer player;

    [SerializeField]
    GameObject model;

    public StatusEffect effect;


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

    private void Start()
    {
        base.Awake();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);
    }
    public override void Awake()
    {   
        //loop through hitboxes, set effect
        if (effect != null)
        {
            Hitbox[] hitboxes = GetComponentsInChildren<Hitbox>();
            foreach (Hitbox hitbox in  hitboxes)
            {
                hitbox.effect = effect;
            }
        }

        player = FindObjectOfType<movePlayer>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        explosionDamageHitBox.gameObject.SetActive(false);

        state = State.Idle;
    }


    void Update()
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
            Debug.Log($"Distance is {distance}, needs to be below {diveRange}");
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

    void Explode()
    {
        explosionDamageHitBox.gameObject.SetActive(true);
        model.gameObject.SetActive(false);
        state = State.Die;
    }

    public void EndRoar()
    {
        state = State.Move;
    }

}
