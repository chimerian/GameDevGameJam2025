using Reflex.Attributes;
using Reflex.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Inject] private readonly Container container;

    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private int tileTypeCount = 5;
    [SerializeField] private int tileSize = 1;
    [SerializeField] private GameObject[] tilePrefabs;

    private TileData[,] tiles;
    private List<TileSolution> solutions;
    private Tile selectedTile;

    private void Start()
    {
        GenerateBoard();
        CheckSolutions();
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
        tiles = new TileData[width, height];

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
        Vector2Int position = new(x, y);
        tile.Initialize(container, position);
        tiles[x, y] = tile.GetTileData();
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
        TileData selectedTileData = selectedTile.GetTileData();
        TileData tileData = tile.GetTileData();

        var dx = Mathf.Abs(selectedTileData.Position.x - tileData.Position.x);
        var dy = Mathf.Abs(selectedTileData.Position.y - tileData.Position.y);
        bool isNeighbor = (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        return isNeighbor;
    }

    private void SwapSelectionTiles(Tile tile)
    {
        TileData selectedTileData = selectedTile.GetTileData();
        TileData tileData = tile.GetTileData();

        //bool isValidMove = CheckMatch(selectedTileData, tileData);

        Direction directionTile;
        Direction directionSelectedTile;

        if (selectedTileData.Position.x > tileData.Position.x)
        {
            directionTile = Direction.Right;
            directionSelectedTile = Direction.Left;
        }
        else if (selectedTileData.Position.x < tileData.Position.x)
        {
            directionTile = Direction.Left;
            directionSelectedTile = Direction.Right;
        }
        else if (selectedTileData.Position.y < tileData.Position.y)
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

    private void CheckSolutions()
    {
        solutions = new();

        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                TileData tile0 = tiles[x, y];
                TileData tileX = tiles[x + 1, y];
                TileData tileY = tiles[x, y + 1];

                SwapTiles(x, y, x + 1, y);
                CheckMatch(tile0.Position, tileX.Position);
                CheckMatch(tileX.Position, tile0.Position);
                SwapTiles(x, y, x + 1, y);

                SwapTiles(x, y, x, y + 1);
                CheckMatch(tile0.Position, tileY.Position);
                CheckMatch(tileY.Position, tile0.Position);
                SwapTiles(x, y, x, y + 1);
            }
        }
    }

    private void SwapTiles(int x1, int y1, int x2, int y2)
    {
        var temp = tiles[x1, y1];
        tiles[x1, y1] = tiles[x2, y2];
        tiles[x2, y2] = temp;
    }

    private IEnumerable<TileSolution> GetSolutions()
    {
        return solutions;
    }

    private void CheckMatch(Vector2Int position, Vector2Int position2)
    {
        TileData tile = tiles[position.x, position.y];

        TileData tileX_2 = position.x >= 2 ? tiles[position.x - 2, position.y] : null;
        TileData tileX_1 = position.x >= 1 ? tiles[position.x - 1, position.y] : null;
        TileData tileX1 = position.x < width - 1 ? tiles[position.x + 1, position.y] : null;
        TileData tileX2 = position.x < width - 2 ? tiles[position.x + 2, position.y] : null;

        TileData tileY_2 = position.y >= 2 ? tiles[position.x, position.y - 2] : null;
        TileData tileY_1 = position.y >= 1 ? tiles[position.x, position.y - 1] : null;
        TileData tileY1 = position.y < height - 1 ? tiles[position.x, position.y + 1] : null;
        TileData tileY2 = position.y < height - 2 ? tiles[position.x, position.y + 2] : null;

        TileSolution tileSolution = solutions.FirstOrDefault(
            s => (s.Position1 == position && s.Position2 == position2) ||
                 (s.Position1 == position2 && s.Position2 == position)
        );

        bool tileSolutionExists = tileSolution != null;
        tileSolution ??= new TileSolution(position, position2);

        if (position.x >= 2 && position.x < width - 2 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType && tileX_2.TileType == tileX1.TileType && tileX_2.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tileX_2.TileType, 5);
        }
        else if (position.x >= 2 && position.x < width - 1 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType && tileX_2.TileType == tileX1.TileType)
        {
            tileSolution.AddTile(tileX_1.TileType, 4);
        }
        else if (position.x >= 1 && position.x < width - 2 && tileX_1.TileType == tile.TileType && tileX_1.TileType == tileX1.TileType && tileX_1.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tileX_1.TileType, 4);
        }
        else if (position.x >= 2 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }
        else if (position.x >= 1 && position.x < width - 1 && tileX_1.TileType == tile.TileType && tileX_1.TileType == tileX1.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }
        else if (position.x < width - 2 && tile.TileType == tileX1.TileType && tile.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }

        if (position.y >= 2 && position.y < height - 2 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType && tileY_2.TileType == tileY1.TileType && tileY_2.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tileY_2.TileType, 5);
        }
        else if (position.y >= 2 && position.y < height - 1 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType && tileY_2.TileType == tileY1.TileType)
        {
            tileSolution.AddTile(tileY_1.TileType, 4);
        }
        else if (position.y >= 1 && position.y < height - 2 && tileY_1.TileType == tile.TileType && tileY_1.TileType == tileY1.TileType && tileY_1.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tileY_1.TileType, 4);
        }
        else if (position.y >= 2 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }
        else if (position.y >= 1 && position.y < height - 1 && tileY_1.TileType == tile.TileType && tileY_1.TileType == tileY1.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }
        else if (position.y < height - 2 && tile.TileType == tileY1.TileType && tile.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3);
        }

        if (!tileSolution.IsEmpty() && !tileSolutionExists)
        {
            solutions.Add(tileSolution);
        }

        if (!tileSolution.IsEmpty())
        {
            Debug.Log(tileSolution);
        }
    }
}