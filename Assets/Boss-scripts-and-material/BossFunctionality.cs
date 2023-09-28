using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFunctionality : MonoBehaviour
{
    enum currentStateOfCombat{
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
    MainComputer mainComputer;
    private int computerHealth;
    [SerializeField]
    GameObject shield;
    public BossEnemy[] be;
    bool releaseEnemies = true;

    currentStateOfCombat state = currentStateOfCombat.TwoTurrets;
    // Start is called before the first frame update
    private void Start()
    {
        computerHealth = mainComputer.Health;
        be = FindObjectsOfType<BossEnemy>();
        foreach (BossEnemy b in be)
        {
            b.gameObject.active = false;
        }
        mainComputer.Health = mainComputer.Health / 2;
    }
    private void Update()
    {
        WaveFSM();
    }
    void WaveFSM()
    {
        switch (state)
        {
            case currentStateOfCombat.TwoTurrets:
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
            case currentStateOfCombat.MiddleFight:
                if (mainComputer.Health <= computerHealth/2)
                {
                    state = currentStateOfCombat.OneTurret;
                    shield.active = true;
                }
                else
                {
                    MiddleFightSequence();
                }
                break;
            case currentStateOfCombat.OneTurret:
                if (turret1.alive == false)
                {
                    state = currentStateOfCombat.OnlyComputer;
                }
                else
                {
                    turret1.ClearToShoot();
                    if (releaseEnemies = false)
                    {
                        SpawnEnemy();
                        releaseEnemies = true;
                    }
                }
                break;
            case currentStateOfCombat.OnlyComputer:
                shield.active = false;
                if (mainComputer.Health <= 0)
                {
                    mainComputer.gameObject.active = false;
                }
                break;
        }
    }
    void MiddleFightSequence()
    {
        shield.active = false;
        if (releaseEnemies)
        {
            SpawnEnemy();
            releaseEnemies = false;
        }
    }
    void SpawnEnemy()
    {
        foreach (BossEnemy enemy in be)
        {
            enemy.gameObject.active = true;
        }
    }
}
