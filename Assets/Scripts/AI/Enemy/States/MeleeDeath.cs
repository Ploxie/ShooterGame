using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Melee/Death")]
public class MeleeDeath : Death
{
    protected EnemyMelee enemyMelee;
    //protected float explosionSize = 0;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyMelee = (EnemyMelee)parent;
    }
    public override void Enter()
    {
        base.Enter();
        if (!enemyMelee.SimulationEnabled)
            enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
        enemyMelee.DE.Effect.Play();
        //AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.deathMelee, enemyMelee.transform.position);
        enemyMelee.DE.StartCoroutine(enemyMelee.DE.DissolveRoutine());

    }
    public override void Update()
    {
        base.Update();
        //if (enemyMelee.DE.counter <= 1)
        //    enemyMelee.DE.StartCoroutine(enemyMelee.DE.DissolveRoutine());
    }
}
