using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    // Start is called before the first frame update

    public void Temp()
    {
        Debug.Log("You did it!");
    }

    private void Explode()
    {
        StateMachine.SetState(typeof(Death));
    }

}
