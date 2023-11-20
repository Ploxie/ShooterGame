using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    private int isFiringHash;
    //Health health; //Om health-- så kör hit animation.
    private Player player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        isFiringHash = Animator.StringToHash("isFiring");
    }

    void Update()
    {
        Animation();
    }

    private void Animation()
    {
        
        bool fire = Input.GetKey(KeyCode.Mouse0);
        bool isFiring = animator.GetBool(isFiringHash);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 lookDirection = new Vector3(player.direction.x, 0f, player.direction.y).normalized;
        float angle = Vector3.Dot(player.moveDirection, lookDirection);
        
        if (x != 0 || z != 0)
        {
            if (angle > 0f)
            {
                //Debug.Log("Angle: " + angle);
                animator.SetBool("isWalking", true);
                animator.SetBool("isWalkingBack", false);
            }
            else if (angle < 0f)
            {
                animator.SetBool("isWalkingBack", true);
                animator.SetBool("isWalking", false);
            } 
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingBack", false);
        }
        if (!isFiring && fire)
        {
            animator.SetBool("isFiring", true);
        }
        if (isFiring && !fire)
        {
            animator.SetBool("isFiring", false);
        }
    }
}
