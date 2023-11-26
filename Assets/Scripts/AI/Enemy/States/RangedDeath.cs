using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Ranged/Death")]
public class RangedDeath : Death
{
    protected EnemyRanged enemyRange;
    //protected float explosionSize = 0;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyRange = (EnemyRanged)parent;
    }
    public override void Enter()
    {
        base.Enter();
        enemy.Agent.isStopped = true;
        enemy.Animator.SetTrigger("Die");
        enemyRange.DissolveEffect.Effect.Play();
        enemyRange.DissolveEffect.StartCoroutine(enemyRange.DissolveEffect.DissolveRoutine());
        AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.deathRanged, enemyRange.transform.position);
    }
    public override void Update()
    {
        base.Update();
        //if (enemyRange.DissolveEffect.counter <= 1)
        //    enemyRange.DissolveEffect.StartCoroutine(enemyRange.DissolveEffect.DissolveRoutine());
    }
}
