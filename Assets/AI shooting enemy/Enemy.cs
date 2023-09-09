using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    public NavMeshAgent agent;
     

    public void MoveToEnemy(movePlayer placeOfPlayer)
    {
        agent.SetDestination(placeOfPlayer.transform.position);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
        //transform.Translate(dir * 1f * Time.deltaTime);
    }
    public void MoveAwayFromEnemy(movePlayer player)
    {
        Vector3 directionOfPlayer = transform.position - player.transform.position;
        
        Vector3 newPosition = transform.position + directionOfPlayer;
        agent.SetDestination(newPosition);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
    }
    public void Shoot()
    {
        agent.Stop();
    }
    public void Patrol(Vector3 patrol)
    {
        if (agent.destination != patrol)
        {
        //    agent.SetDestination(patrol);
        }
        //needs destinations given to it for it to work
    }
}
