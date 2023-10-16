using Assets.Scripts.EventSystem;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFunctionality : Living
{
    EnemyManager enemyManager;
    enum currentStateOfCombat
    {
        TwoTurrets,
        MiddleFight,
        OneTurret,
        OnlyComputer,
    }
    [SerializeField]
    Turret turret1;
    [SerializeField]
    Turret turret2;
    [SerializeField]
    GameObject shield;
    public EnemySpawner[] be;
    bool releaseEnemies = true;
    private EnemyHealthBar healthBar;

    private double lastSpawn;

    private double spawnTimer = 5000;

    currentStateOfCombat state = currentStateOfCombat.TwoTurrets;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Awake();
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);

        healthBar = FindFirstObjectByType<EnemyHealthBar>();

        be = FindObjectsOfType<EnemySpawner>();

        lastSpawn = Utils.GetUnixMillis();
        //mainComputer.Health = mainComputer.Health / 2;
    }
    public override void Update()
    {
        base.Update();
        WaveFSM();
    }
    void WaveFSM()
    {
        switch (state)
        {
            case currentStateOfCombat.TwoTurrets://when two turrets are up.
                if (turret2.alive == true && turret1.alive == true)
                {
                    turret1.ClearToShoot();
                    turret2.ClearToShoot();
                }
                else
                {
                    state = currentStateOfCombat.MiddleFight;
                }
                break;
            case currentStateOfCombat.MiddleFight://fight inbetween one turret destroyed
                if (this.Health <= this.Health / 2)
                {
                    state = currentStateOfCombat.OneTurret;
                    shield.active = true;
                    if (healthBar.enabled)
                    {
                        healthBar.enabled = false;
                    }
                }
                else
                {
                    MiddleFightSequence();
                }
                break;
            case currentStateOfCombat.OneTurret: //when only one turret is left
                if (turret1.alive == false || turret2.alive == false)
                {
                    state = currentStateOfCombat.OnlyComputer;
                }
                else
                {
                    turret1.ClearToShoot();
                    if (releaseEnemies = false)
                    {
                        MiddleFightSequence();
                    }
                }
                break;
            case currentStateOfCombat.OnlyComputer://when only main computer is up
                shield.active = false;
                if (!healthBar.enabled)
                {
                    healthBar.enabled = true;
                }
                break;
        }
    }
    void MiddleFightSequence()
    {
        shield.active = false;

        if (!healthBar.enabled)
        {
            healthBar.enabled = true;
        }
        SpawnEnemy();
    }
    void SpawnEnemy()
    {
        if (Utils.GetUnixMillis() - lastSpawn < spawnTimer)
            return;
        lastSpawn = Utils.GetUnixMillis();
        foreach (EnemySpawner spawn in be)
        {
            spawn.SpawnRandomEnemy();
        }
    }
    public override void TakeDamage(float damage)
    {
        if (!shield.active)
            base.TakeDamage(damage);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        EventManager.TriggerScoreChanged(1000);
        SceneManager.LoadScene("VictoryScreen");
        gameObject.active = false;
    }
}
