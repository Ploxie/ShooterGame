using Assets.Scripts.Entity;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBoss : Enemy
{

    [SerializeField] public List<Turret> Turrets = new List<Turret>();

    public int InitialNumberOfTurrets { get; private set; } = 0;
    [field: SerializeField] public GameObject Shield { get; set; }

    [SerializeField] public EnemySpawner[] SpawnPositions;

    protected override void Start()
    {
        base.Start();
        InitialNumberOfTurrets = Turrets.Count;
    }
    public override void OnHit(float damage, params StatusEffect[] statusEffects)
    {
        if (!Shield.activeSelf)
            base.OnHit(damage, statusEffects);
    }


    protected override void Update()
    {
        base.Update();
    }
    //void WaveFSM()
    //{
    //    switch (state)
    //    {
    //        case currentStateOfCombat.TwoTurrets://when two turrets are up.
    //            if (turret2.alive == true && turret1.alive == true)
    //            {
    //                turret1.ClearToShoot();
    //                turret2.ClearToShoot();
    //            }
    //            else
    //            {
    //                state = currentStateOfCombat.MiddleFight;
    //            }
    //            break;
    //        case currentStateOfCombat.MiddleFight://fight inbetween one turret destroyed
    //            if (this.Health <= this.Health / 2)
    //            {
    //                state = currentStateOfCombat.OneTurret;
    //                shield.active = true;
    //                if (healthBar.enabled)
    //                {
    //                    healthBar.enabled = false;
    //                }
    //            }
    //            else
    //            {
    //                MiddleFightSequence();
    //            }
    //            break;
    //        case currentStateOfCombat.OneTurret: //when only one turret is left
    //            if (turret1.alive == false || turret2.alive == false)
    //            {
    //                state = currentStateOfCombat.OnlyComputer;
    //            }
    //            else
    //            {
    //                turret1.ClearToShoot();
    //                if (releaseEnemies = false)
    //                {
    //                    MiddleFightSequence();
    //                }
    //            }
    //            break;
    //        case currentStateOfCombat.OnlyComputer://when only main computer is up
    //            shield.active = false;
    //            if (!healthBar.enabled)
    //            {
    //                healthBar.enabled = true;
    //            }
    //            break;
    //    }
    //}
    //void MiddleFightSequence()
    //{
    //    shield.active = false;

    //    if (!healthBar.enabled)
    //    {
    //        healthBar.enabled = true;
    //    }
    //    SpawnEnemy();
    //}
    //void SpawnEnemy()
    //{
    //    if (Utils.GetUnixMillis() - lastSpawn < spawnTimer)
    //        return;
    //    lastSpawn = Utils.GetUnixMillis();
    //    foreach (EnemySpawner spawn in SpawnPositions)
    //    {
    //        spawn.SpawnRandomEnemy();
    //    }
    //}
    //public override void TakeDamage(float damage)
    //{
    //    if (!shield.active)
    //        base.TakeDamage(damage);
    //}
    //protected override void OnDeath()
    //{
    //    base.OnDeath();
    //    ScoreManager.Instance?.UpdateText(1000);
    //    SceneManager.LoadScene("VictoryScreen");
    //    gameObject.active = false;
    //}
}

