using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    public Vector3 dir;
    //not worth looking at
    void Update()
    {
        dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(dir * 7f * Time.deltaTime);
    }
}
