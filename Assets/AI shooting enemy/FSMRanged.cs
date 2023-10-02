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
    EnemyRanged enemy;
    [SerializeField]
    Player player;
    Vector3[] paths;
    float distanceShort;
    float distanceLong;
    Vector3 lastSeenPos;
    bool movetolastPos;
    public FSMRanged(EnemyRanged enemy)
    {
        this.enemy = enemy;
        status = states.STATE_PATROL;
    }
    private void Start()
    {
        distanceShort = 10f;
        distanceLong = 30f;
        movetolastPos = true;
        this.player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        //HandelState();//the whole of the switch based state machine
    }
    private void HandelState()
    {
        float distBetweenEnemyPlayer = Vector3.Distance(transform.position, player.transform.position);
        print(status);
        bool seen = enemy.RayCastForVisual(player);
        switch (status)//switch no work we will try the object oriented way. maybe done points on the map where we show where the enemy should be
        {               
            case states.STATE_PATROL://go to last place of enemy and go back to original place. It uses a bool check to keep the patrol state at either look for enemy at a place
                                     //and will return to the original place of the enemy. Will change to shoot state when seeing player with raycast.   
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
                    enemy.rotateToPlayer(lastSeenPos);
                }
                else
                {
                    status = states.STATE_SHOOTING;
                    movetolastPos = false;
                }
                print(movetolastPos);
                break;
            
            
            case states.STATE_SHOOTING:
                
                if(10f >= distBetweenEnemyPlayer && 25f > distBetweenEnemyPlayer)//moves away from player if to close.
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
                else if(25f < distBetweenEnemyPlayer)//move to player if to far away.
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
                break;
            case states.STATE_RUNNING_TO_ENEMY_WHILE_SHOOTING://running towards enemy in this state.
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
            case states.STATE_RUNNING_FROM_ENEMY_WHILE_SHOOTING://running from enemy in this state.
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
