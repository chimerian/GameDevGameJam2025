using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileSolution
{
    private Vector2Int position1;
    private Vector2Int position2;
    private Dictionary<TileType, int> tileCounts;

    public TileSolution(Vector2Int position1, Vector2Int position2)
    {
        this.position1 = position1;
        this.position2 = position2;
        tileCounts = new Dictionary<TileType, int>();
    }

    public Vector2Int Position1 => position1;
    public Vector2Int Position2 => position2;

    public int GetTileCount(TileType tileType)
    {
        if (tileCounts.TryGetValue(tileType, out int count))
        {
            return count;
        }
        return 0;
    }

    public void AddTile(TileType tileType, int count)
    {
        if (!tileCounts.ContainsKey(tileType))
        {
            tileCounts[tileType] = 0;
        }

        tileCounts[tileType] += count;
    }

    public bool IsEmpty()
    {
        return tileCounts.Count == 0;
    }

    public override string ToString()
    {
        string result = $"TileSolution: Position1 = {position1}, Position2 = {position2}, TotalTileCounts = {tileCounts.Values.Sum()}, TileCounts = ";
        result += string.Join(", ", tileCounts.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + ", ";
        return result.TrimEnd(',', ' ');
    }
}