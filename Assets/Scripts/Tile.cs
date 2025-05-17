using UnityEngine;

public class Tile
{
    private GameObject gameObject;
    private TileType tileType;

    public Tile(TileType tileType, GameObject gameObject)
    {
        this.tileType = tileType;
        this.gameObject = gameObject;
    }

    public TileType TileType { get => tileType; }

    public void RefreshTile()
    {
    }
}