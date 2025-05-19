using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject tileSelection;
    [SerializeField] private GameObject tileHighlightSelection;
    [SerializeField] private TileType tileType;
    [SerializeField] private GameObject imageGameObject;

    private Animator animator;
    private Board board;
    private TileData tileData;

    private void Awake()
    {
        tileSelection.SetActive(false);
        tileHighlightSelection.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void Initialize(Container container, Vector2Int position)
    {
        AttributeInjector.Inject(this, container);
        tileData = new TileData(position, tileType, this);
    }

    [Inject]
    public void Construct(Board Board)
    {
        board = Board;
    }

    public TileData GetTileData() => tileData;

    public GameObject GetImageGameObject() => imageGameObject;

    public void ShowHighlightSelection()
    {
        tileHighlightSelection.SetActive(true);
    }

    public void HideHighlightSelection()
    {
        tileHighlightSelection.SetActive(false);
    }

    public void StartIdleAnimation()
    {
        animator.enabled = false;
        imageGameObject.transform.localPosition = Vector3.zero;
        animator.Rebind();
        animator.enabled = true;
        animator.Play("TileIdleAnimation", 0, 0f);
    }

    public void StartSwapAnimation(Direction direction)
    {
        if (direction == Direction.Left)
        {
            animator.SetTrigger("SwapLeft");
        }
        else if (direction == Direction.Right)
        {
            animator.SetTrigger("SwapRight");
        }
        else if (direction == Direction.Up)
        {
            animator.SetTrigger("SwapUp");
        }
        else if (direction == Direction.Down)
        {
            animator.SetTrigger("SwapDown");
        }
    }

    public void StartCollectAnimation(Direction direction)
    {
        if (direction == Direction.Left)
        {
            animator.SetTrigger("MoveLeft");
        }
        else if (direction == Direction.Right)
        {
            animator.SetTrigger("MoveRight");
        }
        else if (direction == Direction.Up)
        {
            animator.SetTrigger("MoveUp");
        }
        else if (direction == Direction.Down)
        {
            animator.SetTrigger("MoveDown");
        }
    }

    public void Collect()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        board.SelectTile(this);
    }

    private void OnMouseEnter()
    {
        ShowSelection();
    }

    private void OnMouseExit()
    {
        HideSelection();
    }

    private void ShowSelection()
    {
        tileSelection.SetActive(true);
    }

    private void HideSelection()
    {
        tileSelection.SetActive(false);
    }
}