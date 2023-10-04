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
    bool walkfwrd = false, walkbwrd = false, idle = true; 
    Animator animator;
    private Vector3 lastSeenPos;
    private bool shouldMoveToLastPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shouldMoveToLastPosition = true;
        player = EnemyManager.Instance.Player;
        enemy = GetComponent<EnemyRanged>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.Player;
        }
        switch (state)//switch no work we will try the object oriented way. maybe done points on the map where we show where the enemy should be
        {
            case State.STATE_PATROL://go to last place of enemy and go back to original place. It uses a bool check to keep the patrol state at either look for enemy at a place
                walkfwrd = true;
                walkbwrd = false;
                idle = false;
                animator.SetBool("WalkForward", walkfwrd);//and will return to the original place of the enemy. Will change to shoot state when seeing player with raycast.   
                HandlePatrolState();
                break;
            case State.STATE_SHOOTING:
                walkfwrd = false;
                walkbwrd = false;
                idle = true;
                animator.SetBool("IdleState", idle);
                HandleShootingState();
                break;
            case State.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING://running towards enemy in this state.
                walkfwrd = true;
                walkbwrd = false;
                idle = false;
                animator.SetBool("WalkForward", walkfwrd);
                HandleRunningToEnemyWhileShootingState();
                break;
            case State.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING://running from enemy in this state.
                walkfwrd = false;
                walkbwrd = true;
                idle = false;
                animator.SetBool("WalkBackwards", true);
                HandleRunningFromEnemyWhileShootingState();
                break;
        }
    }
       
    private void HandleRunningFromEnemyWhileShootingState()
    {
        bool seen = enemy.HasLineOfSightTo(player.transform.position, "Player");
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);

        enemy.MoveAwayFromEnemy(player);
        if (seen == true)
        {
            if (25f <= distBetweenEnemyPlayer)//move from player
            {
                state = State.STATE_SHOOTING;
                
            }
        }
        else
        {
            state = State.STATE_PATROL;
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
        if (seen == true)
        {
            if (10f < distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//move to player
            {
                state = State.STATE_SHOOTING;
            }
        }
        else
        {
            state = State.STATE_PATROL;
            lastSeenPos = player.transform.position;
        }
        print(state);
        //some form of pathfinding with shooting action
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
            }
            else
            {
                state = State.STATE_PATROL;
                lastSeenPos = player.transform.position;
            }
        }
        else if (25f < distBetweenEnemyPlayer)//move to player if to far away.
        {
            if (seen == true)
            {
                state = State.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING;
            }
            else
            {
                state = State.STATE_PATROL;
                lastSeenPos = player.transform.position;
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
            }
        }
    }

    private void HandlePatrolState()
    {
        if (player == null)
        {
            player = EnemyManager.Instance.Player;
        }
        bool seen = enemy.HasLineOfSightTo(player.transform.position, "Player");

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
            shouldMoveToLastPosition = false;
        }
    }
}
