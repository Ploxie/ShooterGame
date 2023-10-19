using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    //public int Health = 100;
    [SerializeField]
    public EnemyRanged ER;
    public Vector3 posOfStart;
    void Start()
    {
        posOfStart = ER.transform.position;
    }
}
