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

    public bool SimulationEnabled;

    public bool Fire;

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
        if (!SimulationEnabled)
            Fire = Input.GetKey(KeyCode.Mouse0);

       

        if (player.IsDashing)
        {
            animator.speed = 0f;
        }
        else
        {
            bool isFiring = animator.GetBool(isFiringHash);
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 lookDirection = new Vector3(player.direction.x, 0f, player.direction.y).normalized;
            float angle = Vector3.Dot(player.moveDirection, lookDirection);
            // Debug.Log(angle);
            Vector3 temp = transform.InverseTransformDirection(player.moveDirection);
            animator.speed = 1f;
            if (x != 0 || z != 0)
            {
                Debug.Log(temp);
                if (temp.x > 0.75f)
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isWalkingBack", false);
                    animator.SetBool("isStrafingLeft", false);
                    animator.SetBool("isStrafingRight", true);
                }
                else if (temp.x < -0.75f)
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isWalkingBack", false);
                    animator.SetBool("isStrafingLeft", true);
                    animator.SetBool("isStrafingRight", false);
                }
                else if (angle > 0f)
                {
                    //Debug.Log("Angle: " + angle);
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isWalkingBack", false);
                    animator.SetBool("isStrafingLeft", false);
                    animator.SetBool("isStrafingRight", false);
                }
                else if (angle < 0f)
                {
                    animator.SetBool("isWalkingBack", true);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isStrafingLeft", false);
                    animator.SetBool("isStrafingRight", false);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isWalkingBack", false);
                animator.SetBool("isStrafingLeft", false);
                animator.SetBool("isStrafingRight", false);
            }
            if (!isFiring && Fire)
            {
                animator.SetBool("isFiring", true);
            }
            if (isFiring && !Fire)
            {
                animator.SetBool("isFiring", false);
            }
        }
    }
}
