

public class ClusterModule : BulletModule
{
    public const int DURABILITY = 500;

    public ClusterModule()
    {
        this.remainingUses = DURABILITY;
    }

    public override BulletEffectData GetData()
    {
        return new BulletEffectData
        {
            Strength = 1
        };
    }

    public override BulletEffect GetBulletEffect()
    {
        return new Cluster(GetData());
    }
}