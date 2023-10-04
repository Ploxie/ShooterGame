using Unity.VisualScripting;
using UnityEngine;

public class Crystal : BulletEffect
{
    public const int DISTANCE_FROM_ORIGINAL = 6;

    public GameObject bulletPrefab;

    private BulletManager bulletManager;

    public Crystal(BulletEffectData data) : base(data) { }

    protected override void OnActivate()
    {
        bulletManager = GameObject.FindObjectOfType<BulletManager>();
        base.OnActivate();
    }

    public override void DoEffect(GameObject hitObject)
    {
        if (hitObject != null)
            return;


        GameObject.Destroy(Parent.gameObject);

        Vector3 position = Parent.transform.position;
        Vector3 direction = -Vector3.Cross(Parent.RigidBody.velocity.normalized, Vector3.up);

        float launchSpeed = Parent.GunController.ModuleController.GetWeaponData().LaunchSpeed;

        Projectile bullet = bulletManager.RequestBullet(Parent.GunController,Parent.Parent, position, Quaternion.identity, true, false);
        bullet.RigidBody.AddRelativeForce(direction * launchSpeed);
        bullet.RigidBody.rotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, direction);

        position = Parent.transform.position;
        bullet = bulletManager.RequestBullet(Parent.GunController,Parent.Parent, position, Quaternion.identity, true, false);
        bullet.RigidBody.AddRelativeForce(-direction * launchSpeed);
        bullet.RigidBody.rotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(90.0f, -direction);

    }
}