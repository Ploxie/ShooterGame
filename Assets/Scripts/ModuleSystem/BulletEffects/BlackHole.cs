using System.Collections.Generic;
using UnityEngine;

public class BlackHole : BulletEffect
{
    public override void DoEffect(GameObject _)
    {
        List<Living> inRange = Parent.EnemyManager.GetEnemiesInRange(Parent.transform.position, 10);
        foreach (Living enemy in inRange)
        {
            Vector3 direction = Parent.transform.position - enemy.transform.position;
            if (direction.magnitude == 0) //Prevent division with 0 in Normalize.
                continue;

            direction.Normalize();
            enemy.GetComponent<Rigidbody>().AddRelativeForce(direction * 100);
        }
    }
}