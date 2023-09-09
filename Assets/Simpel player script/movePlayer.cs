using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class movePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //GameObject go;
    private void Start()
    {
        //go = GetComponent<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(dir * 7f * Time.deltaTime);
        //go.transform.position = transform.position;
    }
}
