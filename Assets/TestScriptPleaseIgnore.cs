using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestScriptPleaseIgnore : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        bool wDown = Input.GetKey(KeyCode.W);
        if (wDown)
        {
            Debug.Log(animator);
            animator.SetBool("IsWalking", true);
        }
        //if (!wDown)
        //{
        //    Debug.Log(animator);
        //    animator.SetBool("IsWalking", false);
        //}

        if (Input.GetKey(KeyCode.Q))
        {
            animator.SetTrigger("Punch");
        }

        if (Input.GetKey(KeyCode.E))
        {
            animator.SetTrigger("JumpAttack");
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                animator.SetBool("IsWalking", true);
            }

            
        }
        if (Vector3.Distance(agent.destination, transform.position) < 1)
        {
            animator.SetBool("IsWalking", false);
        }


    }
}
