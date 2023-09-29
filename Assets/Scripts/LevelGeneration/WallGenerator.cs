using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Assets.Scripts.LevelGeneration.Test2
{
    public static class WallGenerator
    {

        public static IEnumerable<Tile> Generate(IEnumerable<Tile> tiles)
        {
            HashSet<Tile> tileSet = tiles.ToHashSet();
            foreach(Tile tile in tileSet)
            {
                foreach (var direction in Direction2D.CARDINAL)
                {
                    var neighbourPosition = tile.Position + direction;
                    if (!tiles.Contains(new Tile(neighbourPosition)))
                    {
                        tile.Walls |= Tile.Wall.FromDirection(direction);
                    }
                }         
                if(tile.IsCorridor && tile.WallCount >= 2)
                {
                    foreach (var direction in Direction2D.CARDINAL)
                    {
                        var neighbourPosition = tile.Position + direction;
                        Tile neighbour;
                        if (tileSet.TryGetValue(new Tile(neighbourPosition), out neighbour))
                        {
                            if(!neighbour.IsCorridor || neighbour.WallCount == 1)
                                tile.Walls |= Tile.Wall.FromDirection(direction);
                        }
                    }
                }
            }

            return tileSet;
        }

    }
}
