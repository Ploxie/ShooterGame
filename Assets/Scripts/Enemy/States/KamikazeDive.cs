using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Kamikaze/Dive")]
public class KamikazeDive : State
{
    TestEnemy enemy;
    public override void Init(object parent)
    {
        base.Init(parent);
        enemy = (TestEnemy)parent;
    }
    public override void ChangeState()
    {
 
    }

    public override void Exit()
    {
        enemy.Temp();
    }

    public override void Update()
    { 

    }

}
