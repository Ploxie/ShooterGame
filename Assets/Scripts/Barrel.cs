using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : Living
{
    [SerializeField]
    Hitbox explosionDamageHitBox;

    float deathTimer = 0;

    public Assets.Scripts.Entity.StatusEffect effect;

    bool die = false;

    public override void Start()
    {
        base.Start();
        explosionDamageHitBox.gameObject.SetActive(false);
    }
    public override void Awake()
    {
        base.Awake();
        if (effect != null)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<Hitbox>(out Hitbox hitbox))
                {
                    hitbox.Effect = effect;
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if(die)
        {
            deathTimer += Time.deltaTime;
            Debug.Log(deathTimer);
            if (deathTimer > 10)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (Health <= 0)
        {
            explosionDamageHitBox.gameObject.SetActive(true);
            die = true;
        }
    }

}
