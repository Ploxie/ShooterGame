using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    NavMeshAgent agent;
    [SerializeField]
    float jumpSpeedModifier = 7;
    float walkSpeed = 3.5f;
    [SerializeField]
    GameObject effect;

    //DELETE THESE
    Vector3 targetPos = new Vector3(0, 0, 15);
    public Camera cam;

    State state;
    enum State
    {
        Idle,
        Move,
        Jump,
        Attack,
        Die
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {

        if (agent.speed > walkSpeed) 
        {
            agent.speed -= 0.03f; //do something better with deltatime later
        }
        if (agent.velocity.sqrMagnitude > 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                animator.SetTrigger("JumpAttack");
                targetPos = hit.point;
                agent.SetDestination(targetPos);
                agent.speed = jumpSpeedModifier;
                

            }



        }
    }

    public void PlaceEffect()
    {
        GameObject temp = Instantiate(effect);
        temp.transform.position = transform.position;
    }


    private class Jump
    {
        Vector3 end;
        float startDistance, currentDistance, numberThatMakesDistance1;
        public bool active;

        public void SetValues(Vector3 start, Vector3 end)
        {
            active = true;
            this.end = end;
            startDistance = Vector3.Distance(start, end);
            numberThatMakesDistance1 = 1 / startDistance;
        }

        public Vector3 ReturnPointInJump(Vector3 currentPosition)
        {
            if (active)
            {
                currentDistance = Vector3.Distance(end, currentPosition);
                currentPosition.y = Mathf.Sin(currentDistance * numberThatMakesDistance1 * Mathf.PI / 2);
                if (currentDistance < 0)
                {
                    active = false;
                }

            }

            return currentPosition;

        }

    }
}


