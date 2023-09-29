using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : Living
{
    // Start is called before the first frame update
    // Update is called once per frame
    movePlayer player;
    public override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<movePlayer>();
    }

}
