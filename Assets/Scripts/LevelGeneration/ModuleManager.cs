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
                ModulePool[0] = StartModule;
                for (int i = 1; i < RoomsToGenerate; i++)
                    ModulePool[i] = Modules[Random.Range(0, Modules.Length)];
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
