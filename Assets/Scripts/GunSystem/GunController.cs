using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public ModuleController ModuleController;

    //Cached to avoid unnecessary reallocations
    private WeaponData data;
    private double lastFired;
    private GameObject baseProjectile;

    public GunVisual gunVisual;

    public BulletManager bulletManager;

    private AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        ModuleController = GetComponent<ModuleController>();
        lastFired = Utils.GetUnixMillis();
        baseProjectile = Resources.Load<GameObject>("Prefabs/Projectile");
        gunVisual = GetComponent<GunVisual>();
        bulletManager = FindObjectOfType<BulletManager>();
    }

    public void Shoot()
    {
        if (Utils.GetUnixMillis() - lastFired < data.FireRate)
            return;

        Vector3 fireDirection = transform.forward;
        data = ModuleController.GetWeaponData();

        audio.Play();

        foreach (float angle in data.LaunchAngles)
        {

            float angleDeviation = UnityEngine.Random.Range(angle-data.AngleDeviation, angle+data.AngleDeviation);
            Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * fireDirection;
            Vector3 axis = Vector3.Cross(rotatedFireDirection, Vector3.up);

            Projectile bullet = bulletManager.RequestBullet(this, transform.parent.gameObject, gunVisual.GetBarrelPosition(), Quaternion.identity);
            bullet.RigidBody.AddRelativeForce(rotatedFireDirection * data.LaunchSpeed);
            bullet.RigidBody.rotation = Quaternion.AngleAxis(angleDeviation * 0.5f, Vector3.up) * Quaternion.AngleAxis(90.0f, axis);

        }

        lastFired = Utils.GetUnixMillis();
    }
}
