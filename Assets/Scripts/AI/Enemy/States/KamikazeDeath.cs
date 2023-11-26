using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Death")]
public class KamikazeDeath : Death
{
    protected EnemyKamikaze enemyKamikaze;
    protected float explosionSize = 0;

    public override void Init(object parent)
    {
        base.Init(parent);
        enemyKamikaze = (EnemyKamikaze)parent;
    }

    public override void Enter()
    {
        base.Enter();
        enemyKamikaze.Agent.isStopped = true;
        enemyKamikaze.Agent.updateRotation = false;
        //enemyKamikaze.PlaySound("explodekamikaze");
        AudioFmodManager.instance.PlayOneShot(FmodEvents.instance.BoomAndDeathKamikaze, enemyKamikaze.transform.position);
        enemyKamikaze.DE.Effect.Play();
        enemyKamikaze.StartCoroutine(enemyKamikaze.DE.DissolveRoutine());
        enemyKamikaze.Explode();
    }

    public override void Update()
    {
        base.Update();
        //if(enemyKamikaze.DE.counter <= 1)
        //enemyKamikaze.StartCoroutine(enemyKamikaze.DE.DissolveRoutine());    
    }
}
