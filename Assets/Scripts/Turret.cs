using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Living
{
    // Start is called before the first frame update
    //need rotation and 

    private EnemyManager enemyManager;

    public int HealthTurret = 100;
    private Player player;
    public bool alive = true;
    private bool seen = false;

    private EnemyHealthBar healthBar;

    public override void Start()
    {
        base.Start();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);
    }
    public override void Awake()
    {
        base.Awake();
        healthBar = FindFirstObjectByType<EnemyHealthBar>();

        player = FindObjectOfType<Player>();
    }
    public void rotateToPlayer(Vector3 pos)//the enemy rotates to the player 
    {
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
        print(transform.rotation);
    }
    public bool RayCastForVisual(Player player)//a raycast to see if the player is indeed seeing the player
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
    public void Shoot(Player player)//uses a rotation funtion from the quaternion to look for rotation and then to decide where to rotate
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        dir = dir - dir * 2;
        dir = player.transform.position - dir * 3.5f;//changes direction slightly when player moves away to give a sense of in-accuresy
        rotateToPlayer(dir);//player.transform.position);
        //add weapon
    }
    public void isAlive()
    {
        if (Health <= 0f)
        {
            alive = false;
        }
    }
    // Update is called once per frame
    public void ClearToShoot()
    {
        seen = RayCastForVisual(player);
        if (seen == true)
        {
            Shoot(player);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.TakeDamage(damage);
    }
    protected override void OnDeath()
    {

        base.OnDeath();
    }
}
