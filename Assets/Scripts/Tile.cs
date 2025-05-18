using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject tileSelection;
    [SerializeField] private GameObject tileHighlightSelection;
    [SerializeField] private TileType tileType;

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
        tileData = new TileData(position, tileType);
    }

    [Inject]
    public void Construct(Board Board)
    {
        board = Board;
    }

    public TileData GetTileData() => tileData;

    public void HideHighlightSelection()
    {
        tileHighlightSelection.SetActive(false);
    }

    public void StartSwapAnimation(Direction direction)
    {
        //TODO: te animacje s¹ super, ale tylko gdy nie ma dopasowania
        //Je¿eli bêdzie dopasowanie, to powinno byæ tylko "po³owê animacji" + animacja znikania (wybuchu?)
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

    private void OnMouseDown()
    {
        ShowHighlightSelection();
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

    private void ShowHighlightSelection()
    {
        tileHighlightSelection.SetActive(true);
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