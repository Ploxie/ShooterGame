using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
   
    //not worth looking at
    void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(dir * 7f * Time.deltaTime);
        //go.transform.position = transform.position;
    }
}
