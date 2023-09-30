using System;
using UnityEngine;

public class Cluster : BulletEffect
{
    public const int DISTANCE_FROM_CENTER = 1;
    public const int PROJECTILE_COUNT = 8;

    private BulletManager bulletManager;

    protected override void OnActivate()
    {
        bulletManager = GameObject.FindObjectOfType<BulletManager>();
    }

    public override void DoEffect(GameObject hitObject)
    {
        //Replace with event based system with projectile manager in the future
        float placementInterval = 360.0f/PROJECTILE_COUNT;
        for (float i = 0; i < 360; i += placementInterval)
        {
            Vector3 position = hitObject.transform.position + Quaternion.AngleAxis(i, Vector3.up) * new Vector3(0, 0, DISTANCE_FROM_CENTER);
            Vector3 direction = position - hitObject.transform.position;
            if (direction.magnitude == 0) //Prevent division by 0
                continue;
            direction.Normalize();

            GameObject bullet = bulletManager.RequestBullet(position, Quaternion.identity, true, false);
            bullet.GetComponent<Rigidbody>().AddRelativeForce(direction * 1000);
        }
    }
}