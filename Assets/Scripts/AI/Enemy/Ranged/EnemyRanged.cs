using Assets.Scripts.Entity;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyRanged : Enemy
{
    private Gun Gun { get; set; }
    private Module Module { get; set; }

    protected override void Awake()
    {
        base.Awake();
        
        Gun = GetComponent<Gun>();
        Module = Module.CreateRandomModule();
        Gun.ApplyModule(Module);
    }

    public void Shoot()
    {
        Gun?.Shoot();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        PlaySound("deathranged");
        SpawnCartridgePickup(Module);
        StateMachine.SetState(typeof(RangedDeath));
    }

    public bool HasLineOfSightToPlayer() //a raycast to see if the player is indeed seeing the player
    {
        var rayDirection = Player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}

//    [SerializeField] public float Range; // Maybe tie it to the weapon type (shotgun = shorter range?)

//    [Header("Weapon")]
//    [SerializeField] public ModuleController ModuleController;
//    [SerializeField] private ModuleID WeaponID;
//    [SerializeField] private ModuleID EffectID;
//    [SerializeField] private ModuleID BulletID;
//    [SerializeField] CartridgePickup cartridgeDrop;


//    private GunController gunController;
//    private Vector3 spawnPosition;
//    private EnemyHealthBar healthBar;
//    private NavMeshAgent agent;

//    private WeaponModule weaponModule;
//    private EffectModule effectModule;
//    private BulletModule bulletModule;

//    public override void Awake()
//    {
//        base.Awake();
//        healthBar = GetComponentInChildren<EnemyHealthBar>();
//        ModuleController = GetComponentInChildren<ModuleController>();
//        gunController = GetComponentInChildren<GunController>();
//        agent = GetComponent<NavMeshAgent>();

//        weaponModule = (WeaponModule)ModuleRegistry.CreateModuleByID(WeaponID);
//        effectModule = (EffectModule)ModuleRegistry.CreateModuleByID(EffectID);
//        bulletModule = (BulletModule)ModuleRegistry.CreateModuleByID(BulletID);

//        spawnPosition = transform.position;
//    }

//    public override void Start()
//    {
//        base.Start();        
//        EnemyManager.Instance.RegisterEnemy(this);             

//        ModuleController.LoadModule(ModuleType.WeaponModule, weaponModule);
//        ModuleController.LoadModule(ModuleType.EffectModule, effectModule);
//        ModuleController.LoadModule(ModuleType.BulletModule, bulletModule);
//    }

//    public void SwapModules(ModuleID weaponID, ModuleID effectID, ModuleID bulletID)
//    {
//        WeaponID = weaponID;
//        EffectID = effectID;
//        BulletID = bulletID;

//        weaponModule = (WeaponModule)ModuleRegistry.CreateModuleByID(weaponID);
//        effectModule = (EffectModule)ModuleRegistry.CreateModuleByID(effectID);
//        bulletModule = (BulletModule)ModuleRegistry.CreateModuleByID(bulletID);

//        ModuleController.LoadModule(ModuleType.WeaponModule, weaponModule);
//        ModuleController.LoadModule(ModuleType.EffectModule, effectModule);
//        ModuleController.LoadModule(ModuleType.BulletModule, bulletModule);
//    }

//    protected override void OnDeath()
//    {
//        ScoreManager.Instance?.UpdateText(99); // TODO: Eventify
//        Die();
//        Destroy(gameObject);
//    }

//    private void Die()
//    {
//        gameObject.GetComponent<BoxCollider>().enabled = false;

//        if (Health <= 0)
//        {
//            CartridgePickup cartridgeDropInstance = Instantiate(cartridgeDrop, transform.position, Quaternion.identity);

//            var mod = Random.Range(0, 3);
//            ModuleID[] drops = new ModuleID[] { WeaponID, EffectID, BulletID };

//            switch (mod)
//            {
//                case 0:
//                    cartridgeDropInstance.Assign(ModuleType.WeaponModule, drops[mod]);
//                    break;
//                case 1:
//                    cartridgeDropInstance.Assign(ModuleType.EffectModule, drops[mod]);
//                    break;
//                case 2:
//                    cartridgeDropInstance.Assign(ModuleType.BulletModule, drops[mod]);
//                    break;
//                default:
//                    Debug.Log("Haha");
//                    break;
//            }
//        }
//    }


//    public void MoveToEnemy(Player placeOfPlayer)//a function to move towards enemy
//    {
//        RotateTowards(placeOfPlayer.transform.position);
//        agent.SetDestination(placeOfPlayer.transform.position);
//        if (agent.isStopped == true)
//        {
//            agent.Resume();
//        }
//        gunController.Shoot();
//    }
//    public void MoveAwayFromEnemy(Player player)//a function to move away from the enemy
//    {
//        Vector3 directionOfPlayer = transform.position - player.transform.position;

//        Vector3 newPosition = transform.position + directionOfPlayer;
//        RotateTowards(player.transform.position);
//        agent.updateRotation = false;
//        agent.SetDestination(newPosition);
//        if (agent.isStopped == true)
//        {
//            agent.Resume();
//        }
//        gunController.Shoot();
//    }
//    public void Shoot(Player player)//uses a rotation funtion from the quaternion to look for rotation and then to decide where to rotate
//    {
//        ClearLineOfSight(player);
//        if (!agent.isStopped)
//        {
//            agent.Stop();
//        }
//        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//        dir = dir - dir * 2;
//        dir = player.transform.position - dir * 3.5f; //changes direction slightly when player moves away to give a sense of in-accuresy
//        RotateTowards(dir); //player.transform.position);
//        gunController.Shoot();

//    }
//    public void Patrol(Vector3 pos)//a patrol function that looks for the enemy when out of sight when in combination of the FSM
//    {
//        lookAtLastPos(pos);
//        RotateTowards(pos);
//    }
//    public void lookAtLastPos(Vector3 moveToPos)//set destination for last position
//    {
//        agent.SetDestination(moveToPos);
//        agent.Resume();
//    }
//    public bool HasLineOfSightTo(Vector3 position, string tag = "Enemy") //a raycast to see if the player is indeed seeing the player
//    {
//        var rayDirection = position - transform.position;
//        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit))
//        {
//            if (hit.transform.gameObject.tag == tag)
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    public void RotateTowards(Vector3 pos)//the enemy rotates to the player 
//    {
//        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
//        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
//    }
//    public void SimpleLeash()// uses a leach function that mean that the enemy goes back to its original position and makes it idle
//    {
//        agent.Stop();
//        agent.SetDestination(spawnPosition);
//        agent.Resume();
//    }
//    public void ClearLineOfSight(Player player)//looks to see if the a enemy is on the way of its gun arc
//    {

//        var allies = EnemyManager.Instance.GetEnemiesInRange(transform.position, Range);

//        foreach (Living ally in allies)
//        {

//            if (ally == this)
//                continue;


//                if (HasLineOfSightTo(ally.transform.position) == true)
//                {
//                    agent.Stop();
//                    var a = CalculatePath(ally, player);
//                    agent.SetDestination(a);
//                    agent.speed = 6f;
//                    agent.Resume();
//                }
//        }
//    }
//    private Vector3 CalculatePath(Living enemy, Player player)//find the path if left or right is the closer option
//    {
//        var distans = player.transform.position - enemy.transform.position;
//        var left = enemy.transform.position + Vector3.left*15;
//        var right = enemy.transform.position + Vector3.right*15;
//        float x1 = Mathf.Pow(right.x, 2);
//        float x2 = Mathf.Pow(left.x, 2);
//        float x3 = Mathf.Pow(distans.x, 2);

//        Vector3 returnedValue;

//        var a = Mathf.Sqrt(x3 + x1);
//        var b = Mathf.Sqrt(x2 + x3);

//        if(a > b)
//        {
//            returnedValue = left;
//        }
//        else
//        {
//            returnedValue = right;
//        }
//        return returnedValue;
//    }
//    public override void TakeDamage(float damage)
//    {
//        base.TakeDamage(damage);
//        healthBar.TakeDamage(damage); // TODO: Eventify
//    }
//}
