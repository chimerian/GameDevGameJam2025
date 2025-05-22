using Reflex.Attributes;
using System;
using System.Collections.Generic;

public class Player
{
    [Inject] protected readonly TurnSystem turnSystem;

    protected Board board;

    private readonly string name;
    private readonly PlayerType type;
    private readonly Dictionary<TileType, int> points;
    private readonly PointsVisual pointsVisual;

    public Player(string name, PlayerType type, PointsVisual pointsVisual, Board board)
    {
        this.name = name;
        this.type = type;
        this.pointsVisual = pointsVisual;
        this.board = board;

        pointsVisual.SetPlayerName(name);

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

    public virtual void PlayTurn()
    {
    }

    public void AddPoints(TileType tileType, int pointsToAdd)
    {
        points[tileType] += pointsToAdd;
        pointsVisual.SetPoints(tileType, points[tileType]);
    }

    public void ShowBorder()
    {
        pointsVisual.ShowBorder();
    }

    public void HideBorder()
    {
        pointsVisual.HideBorder();
    }

    public int GetPointsCount(TileType tileType)
    {
        return points[tileType];
    }
}