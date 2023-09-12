using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.AI.Navigation;
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
    Vector3 lastSeenPos;
    bool movetolastPos;
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
        movetolastPos = true;
    }
    private void Update()
    {
        HandelState();
    }
    private void HandelState()
    {
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);
        print(status);
        bool seen = enemy.RayCastForVisual(player);
        switch (status)//switch no work we will try the object oriented way. maybe done points on the map where we show where the enemy should be
        {               
            case states.STATE_PATROL://go to last place of enemy and go back to original place.
                if (seen != true)
                {
                    if (movetolastPos == false)
                    {
                        lastSeenPos = player.transform.position;
                        enemy.Patrol(lastSeenPos);
                        movetolastPos=true;
                    }
                    else if(lastSeenPos == enemy.transform.position && movetolastPos == true)
                    {
                        enemy.SimpleLeash();
                    }
                }
                else
                {
                    status = states.STATE_SHOOTING;
                    movetolastPos = false;
                }
                break;
            
            
            case states.STATE_SHOOTING:
                
                if(10f >= distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//move from player
                {
                    if (seen == true)
                    {
                        status = states.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING;
                    }
                    else
                    {
                        status = states.STATE_PATROL;
                        lastSeenPos = player.transform.position;
                    }
                }
                else if(25f < distBetweenEnemyPlayer)//move to player
                {
                    if (seen == true)
                    {
                        status = states.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING;
                    }
                    else
                    {
                        status = states.STATE_PATROL;
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
                        status = states.STATE_PATROL;
                        lastSeenPos = player.transform.position;
                    }
                }
                //shoot at the enemy from standing point and be able to transition to running to/from using enemys movement.
                break;
            case states.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING:
                enemy.MoveToEnemy(player);
                if(seen == true)
                {
                    if (10f < distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//move to player
                    {
                        status = states.STATE_SHOOTING;
                    }
                }
                else
                {
                    status = states.STATE_PATROL;
                    lastSeenPos = player.transform.position;
                }
                print(status);
                //some form of pathfinding with shooting action
                break;
            case states.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING:
                enemy.MoveAwayFromEnemy(player);
                if (seen == true)
                {
                    if (25f <= distBetweenEnemyPlayer)//move from player
                    {
                        status = states.STATE_SHOOTING;
                    }
                }
                else
                {
                    status = states.STATE_PATROL;
                    lastSeenPos = player.transform.position;
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
