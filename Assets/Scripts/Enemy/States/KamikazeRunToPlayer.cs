using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/RunToPlayer")]
public class KamikazeRunToPlayer : RunToPlayer
{
    public override void ChangeState()
    {
        base.ChangeState();
        if (Vector3.Distance(player.transform.position, enemy.transform.position) < 3)
        {
            enemy.Animator.SetTrigger("Dive");
            enemy.StateMachine.SetState(typeof(KamikazeDive));
        }
    }
}
