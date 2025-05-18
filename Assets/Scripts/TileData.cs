using UnityEngine;

public class TileData
{
    private Vector2Int position;
    private TileType tileType;

    public TileData(Vector2Int position, TileType tileType)
    {
        this.Position = position;
        this.tileType = tileType;
    }

    public TileType TileType { get => tileType; }
    public Vector2Int Position { get => position; set => position = value; }

    public override string ToString()
    {
        return $"TileData: Position = {position}, TileType = {tileType}";
    }
}