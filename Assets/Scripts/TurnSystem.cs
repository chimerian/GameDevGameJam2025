using TMPro;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TurnNumberText;
    [SerializeField] private GameObject EndGameGameObject;
    [SerializeField] private TextMeshPro EndGameText;

    private int turnCount = 1;

    private void Start()
    {
        UpdateTurnNumberText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    public int TurnCount => turnCount;

    public void NextTurn()
    {
        turnCount++;
        UpdateTurnNumberText();
    }

    public void ShowEndGame(string name)
    {
        EndGameText.text = $"The winner is {name}!";
        EndGameGameObject.SetActive(true);
    }

    private void UpdateTurnNumberText()
    {
        TurnNumberText.text = $"Turn: {turnCount}";
    }
}