using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public static class RoomManager
    {

        public static RoomModule[] RoomModules;
        public static RoomModule[] StartModules;
        public static RoomModule[] EndModules;
        public static RoomModule[] CorridorModules;
        public static RoomModule[] TutorialModules;

        public static void LoadPrefabs()
        {
            var roomsPath = "Prefabs/Rooms/New/";

            RoomModules = LoadPrefabs(roomsPath + "Random");
            StartModules = LoadPrefabs(roomsPath + "Start");
            EndModules = LoadPrefabs(roomsPath + "End");
            CorridorModules = LoadPrefabs(roomsPath + "Corridor");
            TutorialModules = LoadPrefabs(roomsPath + "Tutorial");

            int roomCount = RoomModules.Length + StartModules.Length + EndModules.Length + CorridorModules.Length;
            Debug.Log("Loaded " + roomCount + " Room Modules");
        }

        private static RoomModule[] LoadPrefabs(string path)
        {
            RoomModule[] temp = Resources.LoadAll<RoomModule>(path);
            foreach (RoomModule roomModule in temp)
            {
                roomModule.CalculateBounds();
            }
            return temp;
            //string[] guids = AssetDatabase.FindAssets("t:prefab", new string[] { path });
            //RoomModule[] result = new RoomModule[guids.Length];

            //for(int i = 0; i < guids.Length; i++)
            //{
            //    var guid = guids[i];
            //    var prefab = AssetDatabase.LoadAssetAtPath<RoomModule>(AssetDatabase.GUIDToAssetPath(guid));

            //    prefab.CalculateBounds();

            //    result[i] = prefab;
            //}

            //return result;
        }        
    }
}
