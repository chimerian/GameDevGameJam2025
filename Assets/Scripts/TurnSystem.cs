using TMPro;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TurnNumberText;

    private int turnCount = 1;

    private void Start()
    {
        UpdateTurnNumberText();
    }

    public int TurnCount => turnCount;

    public void NextTurn()
    {
        turnCount++;
        UpdateTurnNumberText();
    }

    private void UpdateTurnNumberText()
    {
        TurnNumberText.text = $"Turn: {turnCount}";
    }
}