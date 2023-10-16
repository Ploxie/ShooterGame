using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : Enemy
{
    // Start is called before the first frame update
    //need rotation and 

    [Header("Weapon")]
    [SerializeField] public ModuleController ModuleController;
    [SerializeField] private ModuleID WeaponID;
    [SerializeField] private ModuleID EffectID;
    [SerializeField] private ModuleID BulletID;

    private GunController gunController;

    private WeaponModule weaponModule;
    private EffectModule effectModule;
    private BulletModule bulletModule;
    public EnemyBoss Enemy;

    protected override void Awake()
    {
        base.Awake();
        ModuleController = GetComponentInChildren<ModuleController>();
        gunController = GetComponentInChildren<GunController>();

        weaponModule = (WeaponModule)ModuleRegistry.CreateModuleByID(WeaponID);
        effectModule = (EffectModule)ModuleRegistry.CreateModuleByID(EffectID);
        bulletModule = (BulletModule)ModuleRegistry.CreateModuleByID(BulletID);

        ModuleController.LoadModule(ModuleType.WeaponModule, weaponModule);
        ModuleController.LoadModule(ModuleType.EffectModule, effectModule);
        ModuleController.LoadModule(ModuleType.BulletModule, bulletModule);

        Enemy = FindAnyObjectByType<EnemyBoss>();

    }

    public void rotateToPlayer(Vector3 pos)//the enemy rotates to the player 
    {
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
        //print(transform.rotation);
    }
    public bool RayCastForVisual(Assets.Scripts.Entity.Player player)//a raycast to see if the player is indeed seeing the player
    {
       
        bool seePlayer = false;
        var rayDirection = player.transform.position - transform.position;
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject.tag == player.gameObject.tag)
            {
                seePlayer = true;
            }
        }
        //print(this.transform.position - player.transform.position);
        //print(seePlayer);
        return seePlayer;
    }
    public void Shoot(Assets.Scripts.Entity.Player player)//uses a rotation funtion from the quaternion to look for rotation and then to decide where to rotate
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        dir = dir - dir * 2;
        dir = player.transform.position - dir * 3.5f;//changes direction slightly when player moves away to give a sense of in-accuresy
        rotateToPlayer(dir);//player.transform.position);
        gunController.Shoot();
    }
    // Update is called once per frame
    public void ClearToShoot()
    {
        if (RayCastForVisual(Player))
        {
            Shoot(Player);
        }
    }
    protected override void OnDeath()
    {
        Enemy.Turrets.Remove(this);
    }
}
