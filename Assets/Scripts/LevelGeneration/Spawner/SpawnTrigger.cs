using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entity

{
    public class SpawnTrigger : MonoBehaviour
    {
        protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent(out Player p))
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
        }
    }
}
