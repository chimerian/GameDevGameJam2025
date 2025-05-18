using UnityEngine;

public class TileData
{
    private Vector2Int position;
    private TileType tileType;
    private Tile tile;

    public TileData(Vector2Int position, TileType tileType, Tile tile)
    {
        this.Position = position;
        this.tileType = tileType;
        this.tile = tile;
    }

    public TileType TileType { get => tileType; }

    public Vector2Int Position { get => position; set => position = value; }

    public Tile Tile { get => tile; set => tile = value; }

    public override string ToString()
    {
        return $"TileData: Position = {position}, TileType = {tileType}";
    }
}