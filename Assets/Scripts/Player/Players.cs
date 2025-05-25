using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Inject] private readonly TurnSystem turnSystem;
    [Inject] private readonly Board board;
    [Inject] private readonly Anthill anthill;

    [SerializeField] private TextMeshProUGUI PlayerText;
    [SerializeField] private TextMeshPro PlayerTurnText;
    [SerializeField] private Animator PlayerTurnAnimator;
    [SerializeField] private PointsVisual Player1Panel;
    [SerializeField] private PointsVisual Player2Panel;
    [SerializeField] private PointsVisual Player3Panel;
    [SerializeField] private PointsVisual Player4Panel;

    [SerializeField] private List<Button> ActionButtons;

    private readonly List<Player> players = new();
    private Player currentPlayer;

    private void Start()
    {
        players.Add(new Player("PLAYER", PlayerType.Human, Player1Panel, board, this, anthill, turnSystem));
        players.Add(new PlayerAI("CPU 1", PlayerType.AI_Easy, Player2Panel, board, this, anthill, turnSystem));
        players.Add(new PlayerAI("CPU 2", PlayerType.AI_Easy, Player3Panel, board, this, anthill, turnSystem));
        players.Add(new PlayerAI("CPU 3", PlayerType.AI_Easy, Player4Panel, board, this, anthill, turnSystem));
        currentPlayer = players[0];
        UpdateText();
        ShowPlayerTurnText();
        SetupButtons();
    }

    public Player CurrentPlayer => currentPlayer;

    public void NextPlayer()
    {
        currentPlayer.HideBorder();
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

        currentPlayer.ShowBorder();
        StartCoroutine(StartNewPlayerMove());
    }

    public void SetupButtons()
    {
        foreach (Button button in ActionButtons)
        {
            button.SetEnabled(false);

            if (currentPlayer.Type != PlayerType.Human)
            {
                break;
            }

            bool hasEnoughtResources = true;
            for (int i = 0; i < 7; i++)
            {
                if (button.GetCost(i) > currentPlayer.GetPointsCount((ResourceType)i))
                {
                    hasEnoughtResources = false;
                    break;
                }
            }

            if (hasEnoughtResources)
            {
                button.SetEnabled(true);
            }
        }
    }

    private IEnumerator StartNewPlayerMove()
    {
        SetupButtons();

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