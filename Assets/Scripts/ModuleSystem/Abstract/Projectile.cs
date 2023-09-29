using System.Collections.Generic;
using OpenCover.Framework;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float Damage;
    public bool ShouldPierce;
    public int Lifetime;

    protected List<StatusEffect> StatusEffects;
    protected List<BulletEffect> BulletEffects;
    protected HashSet<int> LivingHit;

    protected Rigidbody projectileRigidbody;

    protected double startedLifeAt;

    public EnemyManager EnemyManager;

    public GameEvent OnTickEvent;
    public GameEvent OnHitEvent;
    public GameEvent OnDeleteEvent;

    public void Awake()
    {
        StatusEffects = new List<StatusEffect>();
        BulletEffects = new List<BulletEffect>();
        LivingHit = new HashSet<int>();
        projectileRigidbody = GetComponent<Rigidbody>();
        startedLifeAt = Utils.GetUnixMillis();

        //this NEEDS to be replaced with an event system friendly implementation. This is horrible.
        EnemyManager = FindObjectOfType<EnemyManager>();
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        StatusEffects.Add(effect);
    }

    public void AddBulletEffect(BulletEffect effect)
    {
        effect.Activate(this);
        BulletEffects.Add(effect);
    }

    public void Update()
    {
        if (Utils.GetUnixMillis() - startedLifeAt >= Lifetime)
        {
            Destroy(this.gameObject);
            return;
        }

        foreach (BulletEffect effect in BulletEffects)
            effect.Tick();

        transform.LookAt(projectileRigidbody.velocity);

        OnTick();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Wall")
            return;

        foreach (BulletEffect effect in BulletEffects)
            effect.DoEffect(other.gameObject);

        Living living = other.gameObject.GetComponent<Living>();
        
        if (living != null)
        {
            if (LivingHit.Contains(living.LivingID))
                return;
            
            foreach (StatusEffect effect in StatusEffects)
                living.AddEffect(effect);

            living.TakeDamage(Damage);
            LivingHit.Add(living.LivingID);
        }

        if (!ShouldPierce)
        {
            Destroy(gameObject);
            OnDelete();
        }
        OnHit();           
    }

    protected virtual void OnTick() { }
    protected virtual void OnHit() { }
    protected virtual void OnDelete() { }
}