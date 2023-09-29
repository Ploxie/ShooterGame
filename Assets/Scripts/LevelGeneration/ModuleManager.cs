﻿using Assets.Scripts.LevelGeneration.Test2;
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
        [SerializeField] public RoomModule CorridorModule;
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
                    if(prefab.IsPrefabDefinition())
                        prefabs.Add(prefab);
                }                

                List<RoomModule> available = new List<RoomModule>(prefabs);

                for (int i = 0; i < Modules.Length;i++)
                {
                    Modules[i] = prefabs[Random.Range(0, prefabs.Count)];
                }               
                
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