using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour //a script that utilizes the navmeshagent for pathfinding
 {
    public NavMeshAgent agent;
    Vector3 rubberPosition;

    public void Start()
    {
        rubberPosition = transform.position;
    }

    public void MoveToEnemy(movePlayer placeOfPlayer)//a function to move towards enemy
    {
        rotateToPlayer(placeOfPlayer);
        agent.SetDestination(placeOfPlayer.transform.position);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
        //transform.Translate(dir * 1f * Time.deltaTime);
    }
    public void MoveAwayFromEnemy(movePlayer player)//a function to move away from the enemy
    {
        Vector3 directionOfPlayer = transform.position - player.transform.position;
        
        Vector3 newPosition = transform.position + directionOfPlayer;
        rotateToPlayer(player);
        agent.updateRotation = false;
        agent.SetDestination(newPosition);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
    }
    public void Shoot(movePlayer player)//uses a rotation funtion from the quaternion to look for rotation and then to decide where to rotate
    {
        rotateToPlayer(player);
        agent.Stop();
    }
    public void Patrol(Vector3 pos)//a patrol function that looks for the enemy when out of sight when in combination of the FSM
    {
        lookAtLastPos(pos);
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
        print(this.transform.position - player.transform.position);
        print(seePlayer);
        return seePlayer;
    }
    private void rotateToPlayer(movePlayer player)//the enemy rotates to the player 
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
    }
    public void SimpleLeash()// uses a leach function that mean that the enemy goes back to its original position and makes it idle
    {
        if(agent.isStopped == true)
        {
            agent.Resume();
        }
        agent.SetDestination(rubberPosition);
    }
    
}
