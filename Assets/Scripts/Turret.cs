using Assets.Scripts.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Gun))]
public class Turret : Enemy
{
    private EnemyBoss Boss;

    private Gun Gun { get; set; }
    private Module Module { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Boss = FindAnyObjectByType<EnemyBoss>();

        Gun = GetComponent<Gun>();
        Module = Module.CreateRandomModule();
        Gun.ApplyModule(Module);
    }

    public void RotateToPlayer(Vector3 pos)
    {
        Quaternion rotation = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 60f * Time.deltaTime);
        //print(transform.rotation);
    }

    public bool RayCastForVisual(Assets.Scripts.Entity.Player player)//a raycast to see if the player is indeed seeing the player
    {
       
        bool seePlayer = false;
        var rayDirection = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit))
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
        dir = player.transform.position - dir * 3.5f; //changes direction slightly when player moves away to give a sense of in-accuresy

        RotateToPlayer(dir);//player.transform.position);

        Gun.Shoot();
    }

    public void ClearToShoot()
    {
        if (RayCastForVisual(Player))
        {
            Shoot(Player);
        }
    }

    protected override void OnDeath()
    {
        Boss.Turrets.Remove(this);
        SpawnCartridgePickup(Module);
    }
}
