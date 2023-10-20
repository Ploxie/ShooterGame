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
        public RoomModule[] ModulePool;

        public int RoomsToGenerate;

        

        public void Generate()
        {
            // This should be made more interesting, this was mainly to give the option to create a predefined level with certain modules in order
            ModulePool = new RoomModule[RoomsToGenerate];
            if (Randomize)
            {
                Modules = new RoomModule[Modules.Length];

                var modules = Resources.FindObjectsOfTypeAll<RoomModule>();
                List<RoomModule> prefabs = new List<RoomModule>();
                for(int i = 0; i < Modules.Length; i++)
                {
                    if (!modules[i].enabled)
                        continue;
                    var prefab = modules[i];
                    if(prefab.name != StartModule.name && prefab.name != FinalModule.name && prefab.name != CorridorModule.name)
                        prefabs.Add(prefab);
                }

                ModulePool[0] = StartModule;
                for (int i = 1; i < RoomsToGenerate; i++)
                {
                    ModulePool[i] = prefabs[Random.Range(0, prefabs.Count)];
                }
                ModulePool[ModulePool.Length-1] = FinalModule;
            }
            else
            {
                int pointer = 0;
                for (int i = 0; i < RoomsToGenerate; i++)
                {
                    if (pointer == Modules.Length)
                        pointer = 0;

                    ModulePool[i] = Modules[pointer];
                    pointer++;
                }
            }

            if (ModulePool == null)
                return;

            foreach (RoomModule module in ModulePool)
                module.GenerateTiles();       
        }
    }
}
