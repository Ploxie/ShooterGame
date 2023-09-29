using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : Living
{
    // Start is called before the first frame update
    Animator animator;
    NavMeshAgent agent;
    [SerializeField]
    Hitbox jumpDamageHitBox;
    [SerializeField]
    Hitbox meleeDamageHitBox;

    [SerializeField]
    float jumpSpeedModifier = 7;
    float walkSpeed = 3.5f;

    [SerializeField]
    float detectionRange = 10;
    [SerializeField]
    static float staggerDurationMax = 0.5f;
    float staggerDuration = staggerDurationMax;
    [SerializeField]
    float meleeRange = 220*0.02f;
    [SerializeField]
    float jumpAttackRange = 5;

    private bool canInflictMeleeDamage = false;
    private bool canInflictJumpDamage = false;

    float distance;
    bool isWalking = false;
    bool isDead = false;

    [SerializeField]
    GameObject visualCracks;
    [SerializeField]
    GameObject player;

    public StatusEffect effect;    

    [SerializeField]
    State state;

    enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Die,
        Staggered
    }
    public override void Awake()
    {
        base.Awake();
        //loop through hitboxes, set effect

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        jumpDamageHitBox.gameObject.SetActive(false);
        meleeDamageHitBox.gameObject.SetActive(false);

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        Vector3 fwrd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwrd * 1000, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            Die();
        }

        distance = Vector3.Distance(transform.position, player.transform.position);

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

    private void Die()
    {
        animator.SetTrigger("Die");
        agent.isStopped = true;
        agent.angularSpeed = 0;
        isWalking = false;
        state = State.Die;
        isDead = true;
        animator.SetBool("IsDead", isDead);
        canInflictMeleeDamage = false;
        canInflictJumpDamage = false;
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
            case State.Jump:
                break;
            case State.Attack:
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
            default:
                break;
        }
    }

    private void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);        
        
        if (distance < detectionRange)
        {
            Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit hitInfo);

            if (distance <= meleeRange)
            {
                isWalking = false;

                if (UnityEngine.Random.Range(1, 100) % 2 == 1)
                    animator.SetTrigger("Punch");
                else
                    animator.SetTrigger("Swipe");

                state = State.Attack;
            }
            else if (distance <= jumpAttackRange && distance > jumpAttackRange / 2 && hitInfo.transform.CompareTag("Player"))
            {
                agent.isStopped = false;
                isWalking = false;
                animator.SetTrigger("JumpAttack");
                state = State.Jump;
                agent.speed = jumpSpeedModifier;
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

    public void PlaceEffect()
    {
        GameObject temp = Instantiate(visualCracks);
        temp.transform.position = transform.position;
        temp.transform.Rotate(0, UnityEngine.Random.Range(0, 360f), 0);
    }



    public void ToggleMeleeDamage(int value)
    {
        if (state != State.Die)
        {
            if (value != 0)
            {
                canInflictMeleeDamage = true;
                meleeDamageHitBox.gameObject.SetActive(true);
            }
            else
            {
                canInflictMeleeDamage = false;
                meleeDamageHitBox.gameObject.SetActive(false);
                state = State.Idle;
            }
        }
    }

    public void ToggleJumpDamage(int value)
    {
        if (state != State.Die)
        {
            if (value != 0)
            {
                canInflictJumpDamage = true;
                jumpDamageHitBox.gameObject.SetActive(true);
                PlaceEffect();
            }
            else
            {
                canInflictJumpDamage = false;
                jumpDamageHitBox.gameObject.SetActive(false);
                state = State.Idle;
                agent.speed = walkSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //alternativt, eftersom meleeenemy redan har playerobjektet, applicera skada direkt. mindre intressant utan hitboxar, men betydligt enklare.

        //alternativt alternativt, ha en bool som togglas av animationevents. f�rmodligen det b�ttre alternativet
        switch (state)
        {

            case State.Jump:
                break;
            case State.Attack:
                break;
            default:
                break;
        }
    }    
}


