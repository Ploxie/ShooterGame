using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 1f;
    protected HashSet<int> LivingHit;

    public StatusEffect effect;

    public void Activate()
    {
       LivingHit = new HashSet<int>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Barrel")
            return;

        Living living = other.gameObject.GetComponent<Living>();

        if (living != null)
        {
            if (LivingHit.Contains(living.LivingID))
                return;

            if (effect != null)
                living.AddEffect(effect);

            living.TakeDamage(damage);
            LivingHit.Add(living.LivingID);
        }

    }

}
