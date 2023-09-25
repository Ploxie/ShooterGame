

public class ClusterModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        return new Cluster();
    }
}