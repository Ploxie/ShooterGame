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

        Vector3 fireDirection = transform.rotation * Vector3.forward;
        data = ModuleController.GetWeaponData();

        audio.Play();

        foreach (float angle in data.LaunchAngles)
        {
            float angleDeviation = UnityEngine.Random.Range(angle-data.AngleDeviation, angle+data.AngleDeviation);
            Vector3 rotatedFireDirection = Quaternion.AngleAxis(angleDeviation, Vector3.up) * fireDirection;

            Projectile bullet = bulletManager.RequestBullet(this, gunVisual.GetBarrelPosition(), Quaternion.identity);
            bullet.RigidBody.AddRelativeForce(rotatedFireDirection * data.LaunchSpeed);
        }

        lastFired = Utils.GetUnixMillis();
    }
}
