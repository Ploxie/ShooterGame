using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    private int isWalkingHash;
    private int isWalkingBackHash;
    private int isFiringHash;
    Health health; //Om health-- så kör hit animation.
    Player player;

    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        isFiringHash = Animator.StringToHash("isFiring");
    }

    void Update()
    {
        Animation();
    }

    private void Animation()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        bool fire = Input.GetKey(KeyCode.Mouse0);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isFiring = animator.GetBool(isFiringHash);
        if (!isWalking && (x > 0 || x < 0 || z > 0) && !(z < 0) && (fire || !fire))
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !(x > 0 || x < 0 || z > 0) && (fire || !fire) || (z < 0))
        {
            animator.SetBool(isWalkingHash, false);
        }
        if (!isWalkingBack && ( (z < 0) && !(x > 0 || x < 0 || z > 0) || (z < 0) && (x > 0 || x < 0 || z > 0) ) && (fire || !fire))
        {
            animator.SetBool(isWalkingBackHash, true);
        }
        if (isWalkingBack && !(z < 0) && (fire || !fire))
        {
            animator.SetBool(isWalkingBackHash, false);
        }
        if ( (((z>0) && (z < 0)) || ((x > 0) && (x < 0))) && (fire || !fire))
        {
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isWalkingBackHash, false);
        }
        if (!isFiring && fire)
        {
            animator.SetBool(isFiringHash, true);
        }
        if (isFiring && !fire)
        {
            animator.SetBool(isFiringHash, false);
        }
    }
}
