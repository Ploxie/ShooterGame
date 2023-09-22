using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration.Test2
{
    [Serializable]
    public class RoomGenerator
    {
        public struct Result
        {
            public IEnumerable<Tile> Positions;
        }

        [SerializeField] private int iterations = 10;
        [SerializeField] public int stepCount = 10;
        [SerializeField] public bool startRandomlyEachIteration = true;
       
        public Result Generate(Vector2Int start)
        {
            Vector2Int current = start;
            HashSet<Tile> floorPositions = new HashSet<Tile>();

            for (int i = 0; i < iterations; i++)
            {
                var path = LevelGeneratorObject.SimpleRandomWalk(current, stepCount);
                floorPositions.UnionWith(path);

                if (startRandomlyEachIteration)
                    current = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

            Result result = new Result()
            {
                Positions = floorPositions
            };

            return result;
        }

    }
}
