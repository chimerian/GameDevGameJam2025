using Reflex.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Inject] private readonly TurnSystem turnSystem;

    [SerializeField] private TextMeshProUGUI PlayerText;

    private readonly List<Player> players = new();
    private Player currentPlayer;

    private void Start()
    {
        players.Add(new Player("Player1", PlayerType.Human));
        players.Add(new Player("Player2", PlayerType.AI_Easy));
        currentPlayer = players[0];
        UpdateText();
    }

    public Player CurrentPlayer => currentPlayer;

    public void NextPlayer()
    {
        int currentIndex = players.IndexOf(currentPlayer);
        if (currentIndex == players.Count - 1)
        {
            turnSystem.NextTurn();
            currentPlayer = players[0];
        }
        else
        {
            currentPlayer = players[currentIndex + 1];
        }

        UpdateText();
    }

    private void UpdateText()
    {
        PlayerText.text = $"Player: {players.IndexOf(currentPlayer) + 1} - {currentPlayer.Name} ({currentPlayer.Type})";
    }
}