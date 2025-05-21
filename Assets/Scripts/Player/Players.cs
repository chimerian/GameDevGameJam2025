using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Inject] private readonly TurnSystem turnSystem;
    [Inject] private readonly Board board;

    [SerializeField] private TextMeshProUGUI PlayerText;
    [SerializeField] private TextMeshPro PlayerTurnText;
    [SerializeField] private Animator PlayerTurnAnimator;
    [SerializeField] private PointsVisual Player1Panel;
    [SerializeField] private PointsVisual Player2Panel;
    [SerializeField] private PointsVisual Player3Panel;
    [SerializeField] private PointsVisual Player4Panel;

    private readonly List<Player> players = new();
    private Player currentPlayer;

    private void Start()
    {
        players.Add(new Player("Player1", PlayerType.Human, Player1Panel, board));
        players.Add(new PlayerAI("Player2", PlayerType.AI_Easy, Player2Panel, board));
        //players.Add(new Player("Player3", PlayerType.AI_Easy, Player3Panel));
        //players.Add(new Player("Player4", PlayerType.AI_Easy, Player4Panel));
        currentPlayer = players[0];
        UpdateText();
        ShowPlayerTurnText();
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

        StartCoroutine(StartNewPlayerMove());
    }

    private IEnumerator StartNewPlayerMove()
    {
        ShowPlayerTurnText();
        UpdateText();

        yield return new WaitForSeconds(1f);

        currentPlayer.PlayTurn();
    }

    private void UpdateText()
    {
        PlayerText.text = $"Player: {players.IndexOf(currentPlayer) + 1} - {currentPlayer.Name} ({currentPlayer.Type})";
    }

    private void ShowPlayerTurnText()
    {
        PlayerTurnAnimator.SetTrigger("StartAnimation");
        PlayerTurnText.text = $"{currentPlayer.Name} turn";
    }
}