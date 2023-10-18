using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public float Damage { get; set; }
    public Assets.Scripts.Entity.StatusEffect Effect { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<Collider>().TryGetComponent(out Assets.Scripts.Entity.Player player)) // TODO: Make more generic to handle barrels and such
        {
            player.OnHit(Damage, Effect);            
        }
    }

}
