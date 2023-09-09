using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMRanged : MonoBehaviour
{
    enum states
    {
        STATE_PATROL,
        STATE_SHOOTING,
        STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING,
        STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING
    }

    states status;
    [SerializeField]
    Enemy enemy;
    [SerializeField]
    movePlayer player;
    Vector3[] paths;
    float distanceShort;
    float distanceLong;

    public FSMRanged(Enemy enemy)
    {
        this.enemy = enemy;
        this.player = FindObjectOfType<movePlayer>();
        status = states.STATE_PATROL;
        
    }
    private void Start()
    {
        distanceShort = 10f;
        distanceLong = 30f;
    }
    private void Update()
    {
        HandelState();
    }
    private void HandelState()
    {
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);
        print(status);
        switch (status)//switch no work we will try the object oriented way.
        {               
            case states.STATE_PATROL:
                //should have a view and be able to shange to shooting from this state
                //enemy.Patrol(paths[0]);
                    status = states.STATE_SHOOTING;
                break;
            case states.STATE_SHOOTING:
                if(10f > distBetweenEnemyPlayer && 30f > distBetweenEnemyPlayer)//move from player
                {
                    status = states.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING;
                }
                else if(30f > distBetweenEnemyPlayer)//move to player
                {
                    status = states.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING;
                }
                else 
                {
                    enemy.Shoot();
                }
                //shoot at the enemy from standing point and be able to transition to running to/from using enemys movement.
                break;
            case states.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING:
                enemy.MoveToEnemy(player);
                if (10f > Vector3.Distance(transform.position, player.transform.position) && 30f > distBetweenEnemyPlayer)//move to player
                {
                    status = states.STATE_SHOOTING;
                }
                print(status);
                //some form of pathfinding with shooting action
                break;
            case states.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING:
                enemy.MoveAwayFromEnemy(player);
                if (30f < distBetweenEnemyPlayer)//move from player
                {
                    status = states.STATE_SHOOTING;
                }
                print(status);
                //some form of pathfinding with shooting action
                break;
        }
    }
    // Start is called before the first frame update

    //start by looking at what state they are att and if they are in one they gets checked if they are gonna switch or not.
    //if not in a state it will give it one.
}
