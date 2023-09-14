using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float damage = 1f;

    //public EffectModule effectModule;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Player player = other.GetComponent<Player>();
            //player.DealDamage(damage);
            //player.ApplyEffect(effectModule);
        }
        else if(other.CompareTag("Object"))
        {
            //other.GetComponent<Object>.DealDamage(damage);
        }
    }


}
