using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    protected void OnCollisionEnter(Collision collision)
    {
        foreach (Transform sibling in transform.parent)
        {
            if (sibling.TryGetComponent(out EnemySpawner spawn))
            {
                 spawn.Activate();
            }
        }
        Destroy(gameObject);
    }
}
