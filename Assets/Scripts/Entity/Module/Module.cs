using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entity
{
    [Serializable]
    public abstract class Module
    {
        public GameObject DropPrefab { get; protected set; }

        public Module()
        {
            DropPrefab = Resources.Load<GameObject>("Prefabs/Pickup_Default");
        }

    }
}
