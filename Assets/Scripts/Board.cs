using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Inject] private readonly Container container;

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

    public void SelectTile(Tile tile)
    {
        if (selectedTile == null)
        {
            selectedTile = tile;
        }
        else if (selectedTile == tile)
        {
            return;
        }
        else if (!IsNeighborTile(tile))
        {
            selectedTile.HideHighlightSelection();
            selectedTile = tile;
        }
        else if (IsNeighborTile(tile))
        {
            SwapSelectionTiles(tile);
        }
    }

    private void GenerateBoard()
    {
        tiles = new Tile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
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

        Vector2 localPos = new(-width * tileSize / 2 + x * tileSize, height * tileSize / 2 - (y + 1) * tileSize);
        GameObject tileGameObject = Instantiate(tilePrefabs[(int)tileType], transform);
        tileGameObject.transform.localPosition = localPos;

        Tile tile = tileGameObject.GetComponent<Tile>();
        Vector2 position = new(x, y);
        tile.Initialize(container, position);
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

    private bool IsNeighborTile(Tile tile)
    {
        var dx = Mathf.Abs(selectedTile.Position.x - tile.Position.x);
        var dy = Mathf.Abs(selectedTile.Position.y - tile.Position.y);
        bool isNeighbor = (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        return isNeighbor;
    }

    private void SwapSelectionTiles(Tile tile)
    {
        bool isValidMove = CheckMatch(tile, selectedTile);

        Direction directionTile;
        Direction directionSelectedTile;

        if (selectedTile.Position.x > tile.Position.x)
        {
            directionTile = Direction.Right;
            directionSelectedTile = Direction.Left;
        }
        else if (selectedTile.Position.x < tile.Position.x)
        {
            directionTile = Direction.Left;
            directionSelectedTile = Direction.Right;
        }
        else if (selectedTile.Position.y < tile.Position.y)
        {
            directionTile = Direction.Up;
            directionSelectedTile = Direction.Down;
        }
        else
        {
            directionTile = Direction.Down;
            directionSelectedTile = Direction.Up;
        }

        selectedTile.StartSwapAnimation(directionSelectedTile);
        selectedTile.HideHighlightSelection();
        tile.StartSwapAnimation(directionTile);
        tile.HideHighlightSelection();
        selectedTile = null;
    }

    private bool CheckMatch(Tile tile1, Tile tile2)
    {
        Vector2 tile1Position = tile1.Position;
        Vector2 tile2Position = tile2.Position;

        return false;
    }
}