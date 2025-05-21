using Reflex.Attributes;
using Reflex.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Inject] private readonly Container container;
    [Inject] private readonly Players players;

    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private int tileTypeCount = 5;
    [SerializeField] private int tileSize = 1;
    [SerializeField] private GameObject[] tilePrefabs;

    private TileData[,] tiles;
    private List<TileSolution> solutions;
    private Tile selectedTile;
    private Tile selectedTile2;
    private bool blockMode;

    private void Start()
    {
        GenerateBoard();
        GetSolutions(true);
    }

    public void SelectTile(Tile tile)
    {
        if (blockMode && players.CurrentPlayer.Type == PlayerType.Human)
        {
            return;
        }
        else if (selectedTile == null)
        {
            selectedTile = tile;
            selectedTile.ShowHighlightSelection();
        }
        else if (selectedTile == tile)
        {
            selectedTile.HideHighlightSelection();
            selectedTile = null;
        }
        else if (!IsNeighborTile(tile))
        {
            selectedTile.HideHighlightSelection();
            selectedTile = tile;
            selectedTile.ShowHighlightSelection();
        }
        else if (IsNeighborTile(tile))
        {
            selectedTile2 = tile;
            selectedTile2.ShowHighlightSelection();
            SwapSelectionTiles();
        }
    }

    public TileSolution GetRandomSolution()
    {
        int randomIndex = Random.Range(0, solutions.Count);
        return solutions[randomIndex];
    }

    public Tile GetTile(Vector2Int position)
    {
        return tiles[position.x, position.y].Tile;
    }

    public bool IsBlockedMode()
    {
        return blockMode;
    }

    private void GenerateBoard()
    {
        tiles = new TileData[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                TileType tileType;

                do
                {
                    tileType = (TileType)Random.Range(0, tileTypeCount);
                }
                while (IsNotCorrect(x, y, tileType));

                CreateTile(x, y, tileType);
            }
        }
    }

    private void CreateTile(int x, int y, TileType tileType)
    {
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

    private void GetSolutions(bool swapMode)
    {
        solutions = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (swapMode)
                {
                    if (x < width - 1)
                    {
                        SwapTiles(x, y, x + 1, y);
                        CheckMatch(new Vector2Int(x, y), new Vector2Int(x + 1, y));
                        CheckMatch(new Vector2Int(x + 1, y), new Vector2Int(x, y));
                        SwapTiles(x, y, x + 1, y);
                    }
                    if (y < height - 1)
                    {
                        SwapTiles(x, y, x, y + 1);
                        CheckMatch(new Vector2Int(x, y), new Vector2Int(x, y + 1));
                        CheckMatch(new Vector2Int(x, y + 1), new Vector2Int(x, y));
                        SwapTiles(x, y, x, y + 1);
                    }
                }
                else
                {
                    CheckMatch(new Vector2Int(x, y), new Vector2Int(x, y));
                }
            }
        }
    }

    private void SwapTiles(int x1, int y1, int x2, int y2)
    {
        (tiles[x2, y2], tiles[x1, y1]) = (tiles[x1, y1], tiles[x2, y2]);
    }

    private void CheckMatch(Vector2Int position, Vector2Int position2)
    {
        TileData tile = tiles[position.x, position.y];
        TileData swapTile = tiles[position2.x, position2.y];

        TileData tileX_2 = position.x >= 2 ? tiles[position.x - 2, position.y] : null;
        TileData tileX_1 = position.x >= 1 ? tiles[position.x - 1, position.y] : null;
        TileData tileX1 = position.x < width - 1 ? tiles[position.x + 1, position.y] : null;
        TileData tileX2 = position.x < width - 2 ? tiles[position.x + 2, position.y] : null;

        TileData tileY_2 = position.y >= 2 ? tiles[position.x, position.y - 2] : null;
        TileData tileY_1 = position.y >= 1 ? tiles[position.x, position.y - 1] : null;
        TileData tileY1 = position.y < height - 1 ? tiles[position.x, position.y + 1] : null;
        TileData tileY2 = position.y < height - 2 ? tiles[position.x, position.y + 2] : null;

        TileSolution tileSolution = GetSolution(position, position2);

        bool tileSolutionExists = tileSolution != null;
        tileSolution ??= new TileSolution(position, position2);

        if (position.x >= 2 && position.x < width - 2 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType && tileX_2.TileType == tileX1.TileType && tileX_2.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tileX_2.TileType, 5, new List<Vector2Int> { tileX_2.Position, tileX_1.Position, swapTile.Position, tileX1.Position, tileX2.Position });
        }
        else if (position.x >= 2 && position.x < width - 1 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType && tileX_2.TileType == tileX1.TileType)
        {
            tileSolution.AddTile(tileX_1.TileType, 4, new List<Vector2Int> { tileX_2.Position, tileX_1.Position, swapTile.Position, tileX1.Position });
        }
        else if (position.x >= 1 && position.x < width - 2 && tileX_1.TileType == tile.TileType && tileX_1.TileType == tileX1.TileType && tileX_1.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tileX_1.TileType, 4, new List<Vector2Int> { tileX_1.Position, swapTile.Position, tileX1.Position, tileX2.Position });
        }
        else if (position.x >= 2 && tileX_2.TileType == tileX_1.TileType && tileX_2.TileType == tile.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { tileX_2.Position, tileX_1.Position, swapTile.Position });
        }
        else if (position.x >= 1 && position.x < width - 1 && tileX_1.TileType == tile.TileType && tileX_1.TileType == tileX1.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { tileX_1.Position, swapTile.Position, tileX1.Position });
        }
        else if (position.x < width - 2 && tile.TileType == tileX1.TileType && tile.TileType == tileX2.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { swapTile.Position, tileX1.Position, tileX2.Position });
        }

        if (position.y >= 2 && position.y < height - 2 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType && tileY_2.TileType == tileY1.TileType && tileY_2.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tileY_2.TileType, 5, new List<Vector2Int> { tileY_2.Position, tileY_1.Position, swapTile.Position, tileY1.Position, tileY2.Position });
        }
        else if (position.y >= 2 && position.y < height - 1 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType && tileY_2.TileType == tileY1.TileType)
        {
            tileSolution.AddTile(tileY_1.TileType, 4, new List<Vector2Int> { tileY_2.Position, tileY_1.Position, swapTile.Position, tileY1.Position });
        }
        else if (position.y >= 1 && position.y < height - 2 && tileY_1.TileType == tile.TileType && tileY_1.TileType == tileY1.TileType && tileY_1.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tileY_1.TileType, 4, new List<Vector2Int> { tileY_1.Position, swapTile.Position, tileY1.Position, tileY2.Position });
        }
        else if (position.y >= 2 && tileY_2.TileType == tileY_1.TileType && tileY_2.TileType == tile.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { tileY_2.Position, tileY_1.Position, swapTile.Position });
        }
        else if (position.y >= 1 && position.y < height - 1 && tileY_1.TileType == tile.TileType && tileY_1.TileType == tileY1.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { tileY_1.Position, swapTile.Position, tileY1.Position });
        }
        else if (position.y < height - 2 && tile.TileType == tileY1.TileType && tile.TileType == tileY2.TileType)
        {
            tileSolution.AddTile(tile.TileType, 3, new List<Vector2Int> { swapTile.Position, tileY1.Position, tileY2.Position });
        }

        if (!tileSolution.IsEmpty() && !tileSolutionExists)
        {
            solutions.Add(tileSolution);
        }

        //if (!tileSolution.IsEmpty())
        //{
        //    Debug.Log(tileSolution);
        //}
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

    private void SwapSelectionTiles()
    {
        TileData selectedTileData = selectedTile.GetTileData();
        TileData selectedTileData2 = selectedTile2.GetTileData();

        Direction directionSelectedTile;
        Direction directionSelectedTile2;

        if (selectedTileData.Position.x > selectedTileData2.Position.x)
        {
            directionSelectedTile2 = Direction.Right;
            directionSelectedTile = Direction.Left;
        }
        else if (selectedTileData.Position.x < selectedTileData2.Position.x)
        {
            directionSelectedTile2 = Direction.Left;
            directionSelectedTile = Direction.Right;
        }
        else if (selectedTileData.Position.y < selectedTileData2.Position.y)
        {
            directionSelectedTile2 = Direction.Up;
            directionSelectedTile = Direction.Down;
        }
        else
        {
            directionSelectedTile2 = Direction.Down;
            directionSelectedTile = Direction.Up;
        }

        TileSolution tileSolution = GetSolution(selectedTileData.Position, selectedTileData2.Position);
        if (tileSolution != null)
        {
            SwapSelectedTiles(selectedTileData, selectedTileData2);
            selectedTile.StartCollectAnimation(directionSelectedTile);
            selectedTile2.StartCollectAnimation(directionSelectedTile2);
            blockMode = true;
            StartCoroutine(Collect());
        }
        else
        {
            selectedTile.HideHighlightSelection();
            selectedTile2.HideHighlightSelection();
            selectedTile.StartSwapAnimation(directionSelectedTile);
            selectedTile = null;
            selectedTile2.StartSwapAnimation(directionSelectedTile2);
            selectedTile2 = null;
        }
    }

    private void SwapSelectedTiles(TileData selectedTileData, TileData selectedTileData2)
    {
        SwapTiles(selectedTileData.Position.x, selectedTileData.Position.y, selectedTileData2.Position.x, selectedTileData2.Position.y);
        (selectedTile2.GetTileData().Position, selectedTile.GetTileData().Position) = (selectedTile.GetTileData().Position, selectedTile2.GetTileData().Position);
    }

    private TileSolution GetSolution(Vector2Int position, Vector2Int position2)
    {
        return solutions.FirstOrDefault(
            solution => (solution.Position1 == position && solution.Position2 == position2) ||
                 (solution.Position1 == position2 && solution.Position2 == position)
        );
    }

    private IEnumerator Collect()
    {
        yield return new WaitForSeconds(0.5f);

        selectedTile.HideHighlightSelection();
        selectedTile2.HideHighlightSelection();

        TileData selectedTileData = selectedTile.GetTileData();
        TileData selectedTileData2 = selectedTile2.GetTileData();

        selectedTile.Destroy();
        CreateTile(selectedTileData.Position.x, selectedTileData.Position.y, selectedTileData.TileType);
        selectedTile = null;

        selectedTile2.Destroy();
        CreateTile(selectedTileData2.Position.x, selectedTileData2.Position.y, selectedTileData2.TileType);
        selectedTile2 = null;

        GetSolutions(false);

        do
        {
            foreach (TileSolution solution in solutions)
            {
                Vector2Int position = solution.Position1;
                players.CurrentPlayer.AddPoints(tiles[position.x, position.y].TileType, 1);
                tiles[position.x, position.y].Tile?.Destroy();
                tiles[position.x, position.y] = null;
            }

            MoveDown();
            yield return new WaitForSeconds(0.4f);//animation down time
            GenerateMissingTiles();
            yield return new WaitForSeconds(0.2f);//animation appear time
            GetSolutions(false);

            if (solutions.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);//time for player to see what was created
            }
        }
        while (solutions.Count > 0);

        GetSolutions(true);
        players.NextPlayer();
        if (players.CurrentPlayer.Type == PlayerType.Human)
        {
            blockMode = false;
        }
    }

    private void GenerateMissingTiles()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (tiles[x, y] != null)
                {
                    continue;
                }

                TileType tileType = (TileType)Random.Range(0, tileTypeCount);
                CreateTile(x, y, tileType);
            }
        }
    }

    private void MoveDown()
    {
        Vector2Int[,] fallVectors = new Vector2Int[width, height];

        for (int x = 0; x < width; x++)
        {
            int fallCount = 0;

            for (int y = height - 1; y >= 0; y--)
            {
                if (tiles[x, y] == null)
                {
                    fallCount++;
                }
                else
                {
                    fallVectors[x, y] = new Vector2Int(0, fallCount);
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                Vector2Int fall = fallVectors[x, y];
                if (fall == Vector2Int.zero)
                    continue;

                int newY = y + fall.y;

                TileData tileData = tiles[x, y];
                tileData.Position = new Vector2Int(x, newY);
                tiles[x, newY] = tileData;
                tiles[x, y] = null;

                tileData.Tile.MoveDown(fall.y);
            }
        }
    }
}