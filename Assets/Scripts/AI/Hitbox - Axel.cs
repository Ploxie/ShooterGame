using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [field: SerializeField] public float Damage { get; set; }
    public Assets.Scripts.Entity.StatusEffect Effect { get; set; }

    private List<Assets.Scripts.Entity.Character> hitTargets;
    private bool isMelee = false;

    public bool SimulationEnabled;
    public SimulationHerald Herald;

    public void SetMelee()
    {
        isMelee = true;
    }

    private void OnEnable()
    {
        hitTargets = new List<Assets.Scripts.Entity.Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().TryGetComponent(out Assets.Scripts.Entity.Character target) && !hitTargets.Contains(target)) // TODO: Make more generic to handle barrels and such
        {
            if (!isMelee || target is Assets.Scripts.Entity.Player)
            {
                hitTargets.Add(target);
                target.OnHit(Damage, null, Effect);

                if (SimulationEnabled)
                    Herald.RegisterDamage(DamageRecipient.Player, Damage);
            }
        }
    }

}
