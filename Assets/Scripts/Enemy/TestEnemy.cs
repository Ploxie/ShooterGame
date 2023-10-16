using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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