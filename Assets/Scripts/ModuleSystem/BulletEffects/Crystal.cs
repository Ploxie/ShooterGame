using Unity.VisualScripting;
using UnityEngine;

public class Crystal : BulletEffect
{
    public const int DISTANCE_FROM_ORIGINAL = 6;

    public GameObject bulletPrefab;

    public override void DoEffect(GameObject hitObject)
    {
        if (hitObject != null)
            return;

        //Replace with event based system with projectile manager in the future
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Projectile");

        GameObject.Destroy(Parent.gameObject);
        
        Vector3 position = Parent.transform.position + Quaternion.AngleAxis(270, Vector3.up) * new Vector3(0, 0, DISTANCE_FROM_ORIGINAL);
        Vector3 direction = -Vector3.Cross(Parent.GetComponent<Rigidbody>().velocity.normalized, Vector3.up.normalized);
        
        GameObject bullet = GameObject.Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(direction * 1000);

        Vector3 position2 = Parent.transform.position + Quaternion.AngleAxis(90, Vector3.up) * new Vector3(0, 0, DISTANCE_FROM_ORIGINAL);
        GameObject bullet2 = GameObject.Instantiate(bulletPrefab, position2, Quaternion.identity);
        bullet2.GetComponent<Rigidbody>().AddRelativeForce(-direction * 1000);
    }
}