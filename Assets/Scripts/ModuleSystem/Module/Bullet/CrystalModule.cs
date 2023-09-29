

public class CrystalModule : BulletModule
{
    public override BulletEffect GetBulletEffect()
    {
        Crystal effect = new Crystal();
        effect.ActivationDelay = 500;
        return effect;
    }
}