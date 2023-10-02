using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Search;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration
{
    public class ModuleManager : MonoBehaviour
    {
        [SerializeField] private RoomModule StartModule;
        [SerializeField] public RoomModule CorridorModule;
        [SerializeField] private RoomModule FinalModule;

        [SerializeField] public bool Randomize;
        [SerializeField] public RoomModule[] Modules;

        private void OnValidate()
        {
            // This should be made more interesting, this was mainly to give the option to create a predefined level with certain modules in order
            if(Randomize)
            {
                Modules = new RoomModule[Modules.Length];

                var modules = Resources.FindObjectsOfTypeAll<RoomModule>();
                List<RoomModule> prefabs = new List<RoomModule>();
                for(int i = 0; i < modules.Length; i++)
                {
                    var prefab = modules[i];
                    if(prefab.IsPrefabDefinition() && prefab.name != StartModule.name && prefab.name != FinalModule.name && prefab.name != CorridorModule.name)
                        prefabs.Add(prefab);
                }                

                List<RoomModule> available = new List<RoomModule>(prefabs);

                Modules[0] = StartModule;
                for (int i = 1; i < Modules.Length-1;i++)
                {
                    Modules[i] = prefabs[Random.Range(0, prefabs.Count)];
                }
                Modules[Modules.Length-1] = FinalModule;
            }

            if (Modules == null)
                return;

            foreach(RoomModule module in Modules)
            {
                module.GenerateTiles();
            }                      
        }

    }
}
