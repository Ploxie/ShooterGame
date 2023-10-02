using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    [AddComponentMenu("Level Generator/PlayerSpawner")]
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

        private void Start()
        {
            SpawnPlayer();
        }

        public void SpawnPlayer()
        {
            if (player == null)
                return;

            Instantiate(player, transform.position, Quaternion.identity);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, new Vector3(0.5f, 1, 0.5f));
        }
    }
}
