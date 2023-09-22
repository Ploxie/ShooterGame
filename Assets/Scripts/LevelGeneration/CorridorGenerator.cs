using Assets.Scripts.LevelGeneration.Test2.Editor;
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
    public class CorridorGenerator
    {
        public struct Result
        {
            public IEnumerable<Tile> Positions;
            public IEnumerable<Tile> RoomPositions;
        }

        [SerializeField, MinMaxIntSlider(1, 10)] private Vector2Int corridorLength;
        [SerializeField] private int corridorCount;

        public Result Generate(Vector2Int start)
        {
            HashSet<Tile> positions = new HashSet<Tile>();
            HashSet<Tile> roomPositions = new HashSet<Tile>();
            Tile current = new Tile(start);
            for (int i = 0; i < corridorCount; i++)
            {
                var corridor = LevelGeneratorObject.RandomWalkCorridor(current, Random.Range(corridorLength.x, corridorLength.y));
                current = corridor.Last();
                roomPositions.Add(current);
                positions.UnionWith(corridor);
            }

            Result result = new Result()
            {
                Positions = positions,
                RoomPositions = roomPositions
            };

            return result;
        }
    }
}
