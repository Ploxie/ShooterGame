using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Entity
{
    public class TestEnemy : Enemy
    {
        
        protected override void OnDeath()
        {
            base.OnDeath();
            SpawnCartridgePickup(new PistolWeapon());
        }

    }
}
