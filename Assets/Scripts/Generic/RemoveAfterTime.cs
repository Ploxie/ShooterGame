using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAfterTime : MonoBehaviour
{
    [SerializeField] private double timer;
    private double startTime;
    private void Awake()
    {
        startTime = Utils.GetUnixMillis();
    }

    // Update is called once per frame
    void Update()
    {
        if (Utils.GetUnixMillis() - startTime >= timer)
        {
            Destroy(gameObject);
        }
    }
}
