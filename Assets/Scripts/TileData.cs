using UnityEngine;

public class TileData
{
    private Vector2Int position;
    private ResourceType tileType;
    private Tile tile;

    public TileData(Vector2Int position, ResourceType tileType, Tile tile)
    {
        this.Position = position;
        this.tileType = tileType;
        this.tile = tile;
    }

    public ResourceType TileType { get => tileType; }

    public Vector2Int Position { get => position; set => position = value; }

    public Tile Tile { get => tile; set => tile = value; }

    public override string ToString()
    {
        return $"TileData: Position = {position}, TileType = {tileType}";
    }
}