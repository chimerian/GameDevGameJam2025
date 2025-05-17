using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private int tileTypeCount = 5;
    [SerializeField] private int tileSize = 1;
    [SerializeField] private GameObject[] tilePrefabs;

    private int[,] board;

    private void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        board = new int[width, height];

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
        int tileId;

        do
        {
            tileId = Random.Range(0, tileTypeCount);
        }
        while (IsNotCorrect(x, y, tileId));

        board[x, y] = tileId;

        Vector2 localPos = new(-width * tileSize / 2 + x * tileSize, -height * tileSize / 2 + y * tileSize);
        GameObject tile = Instantiate(tilePrefabs[tileId], transform);
        tile.transform.localPosition = localPos;
    }

    private bool IsNotCorrect(int x, int y, int tileId)
    {
        if (x >= 2 &&
            board[x - 1, y] == tileId &&
            board[x - 2, y] == tileId)
        {
            return true;
        }

        if (y >= 2 &&
            board[x, y - 1] == tileId &&
            board[x, y - 2] == tileId)
        {
            return true;
        }

        return false;
    }
}