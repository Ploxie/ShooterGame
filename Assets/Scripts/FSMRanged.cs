using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.AI.Navigation;
using UnityEngine;


public class FSMRanged : MonoBehaviour
{
    enum State
    {
        STATE_PATROL,
        STATE_SHOOTING,
        STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING,
        STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING
    }

    private State state;
    private EnemyRanged enemy;
    private Player player;
    bool walkfwrd = false, walkbwrd = false, idle = false, shangedIdle = false, shangedfwrd = false, shangedBackwads = false, changedPatrol = false;//animations 
    Animator animator;
    private Vector3 lastSeenPos;
    private bool shouldMoveToLastPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shouldMoveToLastPosition = true;
        //player = FindObjectOfType<Player>();

        enemy = GetComponent<EnemyRanged>();
    }
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        switch (state)//switch no work we will try the object oriented way. maybe done points on the map where we show where the enemy should be
        {
            case State.STATE_PATROL://go to last place of enemy and go back to original place. It uses a bool check to keep the patrol state at either look for enemy at a place
                if (changedPatrol == true)
                {
                    walkfwrd = false;
                    walkbwrd = true;
                    idle = false;
                    changedPatrol = false;
                    animator.SetBool("IdleState", idle);
                    animator.SetBool("WalkForward", walkfwrd);
                    animator.SetBool("WalkBackwards", walkbwrd);
                }//and will return to the original place of the enemy. Will change to shoot state when seeing player with raycast.   
                HandlePatrolState();
                break;
            case State.STATE_SHOOTING:
                if (shangedIdle == true)
                {
                    walkfwrd = false;
                    walkbwrd = false;
                    idle = true;
                    shangedIdle = false;
                    animator.SetBool("IdleState", idle);
                    animator.SetBool("WalkForward", walkfwrd);
                    animator.SetBool("WalkBackwards", walkbwrd);
                }
                HandleShootingState();
                break;
            case State.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING://running towards enemy in this state.

                if (shangedfwrd == true)
                {
                    walkfwrd = true;
                    walkbwrd = false;
                    idle = false;
                    shangedfwrd = false;
                    animator.SetBool("IdleState", idle);
                    animator.SetBool("WalkForward", walkfwrd);
                    animator.SetBool("WalkBackwards", walkbwrd);
                }
                HandleRunningToEnemyWhileShootingState();
                break;
            case State.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING://running from enemy in this state.
                if (shangedBackwads == true)
                {
                    walkfwrd = false;
                    walkbwrd = true;
                    idle = false;
                    shangedBackwads = false;
                    animator.SetBool("IdleState", idle);
                    animator.SetBool("WalkForward", walkfwrd);
                    animator.SetBool("WalkBackwards", walkbwrd);
                }
                HandleRunningFromEnemyWhileShootingState();
                break;
        }
        //}

    }

    private void HandleRunningFromEnemyWhileShootingState()
    {
        bool seen = enemy.HasLineOfSightTo(player.transform.position, "Player");
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);

        enemy.MoveAwayFromEnemy(player);
        //enemy.Shoot(player);
        if (seen == true)
        {
            if (25f <= distBetweenEnemyPlayer)//move from player
            {
                state = State.STATE_SHOOTING;
                shangedIdle = true;
            }
        }
        else
        {
            state = State.STATE_PATROL;
            changedPatrol = true;
            lastSeenPos = player.transform.position;
        }
        print(state);
        //some form of pathfinding with shooting action
    }

    private void HandleRunningToEnemyWhileShootingState()
    {
        bool seen = enemy.HasLineOfSightTo(player.transform.position, "Player");
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);

        enemy.MoveToEnemy(player);
        //enemy.Shoot(player);
        if (seen == true)
        {
            if (10f < distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//move to player
            {
                state = State.STATE_SHOOTING;
                shangedIdle = true;
            }
        }
        else
        {
            state = State.STATE_PATROL;
            shangedfwrd = true;
            lastSeenPos = player.transform.position;
        }
    }

    private void HandleShootingState()
    {
        bool seen = enemy.HasLineOfSightTo(player.transform.position, "Player");
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (10f >= distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//moves away from player if to close.
        {
            if (seen == true)
            {
                state = State.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING;
                shangedBackwads = true;
            }
            else
            {
                state = State.STATE_PATROL;
                changedPatrol = true;
                lastSeenPos = player.transform.position;
            }
        }
        else if (25f < distBetweenEnemyPlayer)//move to player if to far away.
        {
            if (seen == true)
            {
                state = State.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING;
                shangedfwrd = true;
            }
            else
            {
                state = State.STATE_PATROL;
                lastSeenPos = player.transform.position;
                changedPatrol = true;
            }
        }
        else
        {
            if (seen == true)
            {
                enemy.Shoot(player);
            }
            else
            {
                state = State.STATE_PATROL;
                lastSeenPos = player.transform.position;
                changedPatrol = true;
            }
        }
    }

    private void HandlePatrolState()
    {
        bool seen = enemy.HasLineOfSightTo(player.transform.position, player.gameObject.tag);

        if (seen != true)
        {
            if (shouldMoveToLastPosition == false)
            {
                lastSeenPos = player.transform.position;
                enemy.Patrol(lastSeenPos);
                shouldMoveToLastPosition = true;
            }
            else if (lastSeenPos == enemy.transform.position && shouldMoveToLastPosition == true)
            {
                enemy.SimpleLeash();
            }
            enemy.RotateTowards(lastSeenPos);
        }
        else
        {
            state = State.STATE_SHOOTING;
            changedPatrol = true;
            shouldMoveToLastPosition = false;
        }
    }
}
