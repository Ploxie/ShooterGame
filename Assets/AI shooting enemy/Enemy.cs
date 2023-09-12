using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    public NavMeshAgent agent;
    NavMeshSurface NMS;
    Vector3 rubberPosition;

    public void Start()
    {
        this.NMS = FindObjectOfType<NavMeshSurface>();
        rubberPosition = transform.position;
    }

    public void MoveToEnemy(movePlayer placeOfPlayer)
    {
        rotateToPlayer(placeOfPlayer);
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
        rotateToPlayer(player);
        agent.updateRotation = false;
        agent.SetDestination(newPosition);
        if (agent.isStopped == true)
        {
            agent.Resume();
        }
    }
    public void Shoot(movePlayer player)
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 30f * Time.deltaTime);
        agent.Stop();
    }
    public void Patrol(Vector3 pos)
    {
        lookAtLastPos(pos);
    }
    public void lookAtLastPos(Vector3 moveToPos)
    {
        agent.SetDestination(moveToPos);
        agent.Resume();
    }
    public bool RayCastForVisual(movePlayer player)
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
    private void rotateToPlayer(movePlayer player)
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 30f * Time.deltaTime);
    }
    public void SimpleLeash()
    {
        if(agent.isStopped == true)
        {
            agent.Resume();
        }
        agent.SetDestination(rubberPosition);
    }
    
}
