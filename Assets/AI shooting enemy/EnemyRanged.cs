using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyRanged : MonoBehaviour //a script that utilizes the navmeshagent for pathfinding
 {
    public NavMeshAgent agent;
    Vector3 rubberPosition;
    public int Health = 100;
    ShootingLogic SL;
    private EnemyRanged[] enemies;

    public void Start()
    {
        SL = new ShootingLogic();
        rubberPosition = transform.position;
        enemies = FindObjectsOfType<EnemyRanged>();
    }

    public void MoveToEnemy(movePlayer placeOfPlayer)//a function to move towards enemy
    {
        rotateToPlayer(placeOfPlayer.transform.position);
        agent.SetDestination(placeOfPlayer.transform.position);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
    }
    public void looseHealth()
    {
        Health = Health - 10;
    }
    public void MoveAwayFromEnemy(movePlayer player)//a function to move away from the enemy
    {
        Vector3 directionOfPlayer = transform.position - player.transform.position;
        
        Vector3 newPosition = transform.position + directionOfPlayer;
        rotateToPlayer(player.transform.position);
        agent.updateRotation = false;
        agent.SetDestination(newPosition);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
    }
    public void Shoot(movePlayer player)//uses a rotation funtion from the quaternion to look for rotation and then to decide where to rotate
    {
        ClearLineOfSight(player);
        if (!agent.isStopped)
        {
            agent.Stop();
        }
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        dir = dir - dir*2;
        dir = player.transform.position - dir * 3.5f;//changes direction slightly when player moves away to give a sense of in-accuresy
        rotateToPlayer(dir);//player.transform.position);
        //add weapon
    }
    public void Patrol(Vector3 pos)//a patrol function that looks for the enemy when out of sight when in combination of the FSM
    {
        lookAtLastPos(pos);
        rotateToPlayer(pos);
    }
    public void lookAtLastPos(Vector3 moveToPos)//set destination for last position
    {
        agent.SetDestination(moveToPos);
        agent.Resume();
    }
    public bool RayCastForVisual(movePlayer player)//a raycast to see if the player is indeed seeing the player
    {

        bool seePlayer = false;
        var rayDirection = player.transform.position - transform.position;
        RaycastHit hit = new RaycastHit();
        
        if(Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if(hit.transform.gameObject.tag == player.gameObject.tag)
            {
                seePlayer = true;
            }
        }
        //print(this.transform.position - player.transform.position);
        //print(seePlayer);
        return seePlayer;
    }
    public bool RayCastForEnemy(EnemyRanged enemy)//a raycast to see if the player is indeed seeing the player
    {

        bool enemyInTheWay = false;
        var rayDirection = enemy.transform.position - transform.position;
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject.tag == enemy.gameObject.tag)
            {
                enemyInTheWay = true;
            }
        }
        return enemyInTheWay;
    }
    public void rotateToPlayer(Vector3 pos)//the enemy rotates to the player 
    {
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
        //print(transform.rotation);
    }
    public void SimpleLeash()// uses a leach function that mean that the enemy goes back to its original position and makes it idle
    {
        agent.Stop();
        agent.SetDestination(rubberPosition);
        agent.Resume();
    }
    public void ClearLineOfSight(movePlayer player)//looks to see if the a enemy is on the way of its gun arc
    {
        foreach (EnemyRanged m in enemies)
        {
            if (m.gameObject != gameObject)
            {
                if (RayCastForEnemy(m) == true)
                {
                    agent.Stop();
                    var a = calulatePath(m, player);
                    agent.SetDestination(a);
                    agent.speed = 6f;
                    agent.Resume();
                }
            }
        }
    }
    private Vector3 calulatePath(EnemyRanged enemy, movePlayer player)//find the path if left or right is the closer option
    {
        var distans = player.transform.position - enemy.transform.position;
        var left = enemy.transform.position + Vector3.left*15;
        var right = enemy.transform.position + Vector3.right*15;
        float x1 = Mathf.Pow(right.x, 2);
        float x2 = Mathf.Pow(left.x, 2);
        float x3 = Mathf.Pow(distans.x, 2);

        Vector3 returnedValue;

        var a = Mathf.Sqrt(x3 + x1);
        var b = Mathf.Sqrt(x2 + x3);

        if(a > b)
        {
            returnedValue = left;
        }
        else
        {
            returnedValue = right;
        }
        return returnedValue;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (Health <= 0)
            {
                gameObject.active = false;
            }
            else
            {
                looseHealth();
            }
        }
    }
}
