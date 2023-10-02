using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 PositionAheadOfTime(movePlayer mp)
    {
        var pos = mp.transform.position * 1.2f;
        var p = mp.transform.position - pos;
        return (p);
    }
}
