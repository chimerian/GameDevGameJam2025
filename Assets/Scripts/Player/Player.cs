using System.Collections.Generic;

public class Player
{
    private readonly string name;
    private readonly PlayerType type;
    private readonly Dictionary<TileType, int> points;

    public Player(string name, PlayerType type)
    {
        this.name = name;
        this.type = type;

        points = new Dictionary<TileType, int>
        {
            { TileType.type1, 0 },
            { TileType.type2, 0 },
            { TileType.type3, 0 },
            { TileType.type4, 0 },
            { TileType.type5, 0 }
        };
    }

    public string Name => name;

    public PlayerType Type => type;

    public void AddPoints(TileType tileType, int pointsToAdd)
    {
        points[tileType] += pointsToAdd;
    }

    public int GetPointsCount(TileType tileType)
    {
        return points[tileType];
    }
}