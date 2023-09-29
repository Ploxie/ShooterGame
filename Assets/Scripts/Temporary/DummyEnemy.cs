using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Everything is public since this is a dummy version that wont be used
public class DummyEnemy : Living
{
    private EnemyManager enemyManager;

    public void Start()
    {
        //this NEEDS to be replaced with an event system friendly implementation. This is horrible.
        enemyManager = FindObjectOfType<EnemyManager>();
        enemyManager.RegisterEnemy(this);
        base.Awake();
    }


}
