using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private int tileTypeCount = 5;
    [SerializeField] private int tileSize = 1;
    [SerializeField] private GameObject[] tilePrefabs;

    private Tile[,] tiles;
    private Tile selectedTile;

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateTile(x, y);
            }
        }
    }

    private void CreateTile(int x, int y)
    {
        TileType tileType;

        do
        {
            tileType = (TileType)Random.Range(0, tileTypeCount);
        }
        while (IsNotCorrect(x, y, tileType));

        CreateTile(x, y, tileType);
    }

    private void CreateTile(int x, int y, TileType tileType)
    {
        Vector2 localPos = new(-width * tileSize / 2 + x * tileSize, -height * tileSize / 2 + y * tileSize);
        GameObject tileGameObject = Instantiate(tilePrefabs[(int)tileType], transform);
        tileGameObject.transform.localPosition = localPos;

        Tile tile = new(tileType, tileGameObject);
        tiles[x, y] = tile;
    }

    private bool IsNotCorrect(int x, int y, TileType tileType)
    {
        if (x >= 2 &&
            tiles[x - 1, y].TileType == tileType &&
            tiles[x - 2, y].TileType == tileType)
        {
            return true;
        }

        if (y >= 2 &&
            tiles[x, y - 1].TileType == tileType &&
            tiles[x, y - 2].TileType == tileType)
        {
            return true;
        }

        return false;
    }
}