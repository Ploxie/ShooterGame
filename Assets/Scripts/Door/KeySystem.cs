using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySystem : MonoBehaviour
{

    public List<GameObject> keys = new List<GameObject>();

    private void OnTriggerEnter(Collider objectYouHit)
    {
        if (objectYouHit.gameObject.name == "BlueKey")
        {
            keys.Add(objectYouHit.gameObject);
            Destroy(objectYouHit.gameObject);
        }
    }
}