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
        enemyKamikaze.PlaySound("explodekamikaze");
        enemyKamikaze.Explode();
    }

    public override void Update()
    {
        explosionSize += Time.deltaTime / 10;
        enemyKamikaze.ExplosionDamageHitBox.transform.localScale += new Vector3(explosionSize, explosionSize, explosionSize);
        base.Update();
    }
}
