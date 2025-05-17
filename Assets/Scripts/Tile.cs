using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject tileSelection;
    [SerializeField] private GameObject tileHighlightSelection;
    [SerializeField] private TileType tileType;

    private Board Board;

    public TileType TileType { get => tileType; }

    public void Initialize(Container container)
    {
        AttributeInjector.Inject(this, container);
    }

    [Inject]
    public void Construct(Board Board)
    {
        this.Board = Board;
    }

    public void HideHighlightSelection()
    {
        tileHighlightSelection.SetActive(false);
    }

    private void OnMouseDown()
    {
        Board.SelectTile(this);
        ShowHighlightSelection();
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