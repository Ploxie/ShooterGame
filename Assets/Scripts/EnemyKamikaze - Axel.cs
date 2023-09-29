using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKamikaze : Living
{
    private EnemyManager enemyManager;

    Animator animator;
    NavMeshAgent agent;
    [SerializeField]
    Hitbox explosionDamageHitBox;

    [SerializeField]
    float detectionRange = 10;
    [SerializeField]
    static float staggerDurationMax = 0.5f;
    float staggerDuration = staggerDurationMax;

    [SerializeField]
    float diveRange = 10;

    float distance;
    bool isWalking = false;
    bool isDead = false;
    bool playerDetected = false;

    [SerializeField]
    GameObject player;

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
    public override void Awake()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);
        base.Awake();
        //loop through hitboxes, set effect

        if (effect != null)
        {
            foreach (Transform t in transform)
            {
                if (t.CompareTag("HitBox"))
                {
                    t.GetComponent<Hitbox>().effect = effect;
                }
            }
        }

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
            Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit hitInfo);
            if (distance <= diveRange && hitInfo.transform.CompareTag("Player") && state == State.Move)
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
