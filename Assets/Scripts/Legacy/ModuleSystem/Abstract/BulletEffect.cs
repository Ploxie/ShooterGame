

using UnityEngine;

public abstract class BulletEffect
{
    public Projectile Parent;

    public int ActivationDelay;
    
    protected double activatedAt;
    protected bool hasFired;
    protected bool activated;

    protected BulletEffectData effectData;

    protected BulletEffect(BulletEffectData data)
    {
        effectData = data;
        ActivationDelay = -1;
        activatedAt = 0;
        hasFired = false;
        activated = false;
    }

    public void Activate(Projectile parent)
    {
        Parent = parent;
        activatedAt = Utils.GetUnixMillis();
        activated = true;
        OnActivate();
    }

    public void Tick()
    {
        if (!hasFired && activated && ActivationDelay != -1 && Utils.GetUnixMillis() - activatedAt >= ActivationDelay)
        {
            DoEffect(null);
            hasFired = true;
        }

        OnTick();
    }

    protected virtual void OnActivate() { }
    protected virtual void OnTick() { }
    public abstract void DoEffect(GameObject hitObject);
}