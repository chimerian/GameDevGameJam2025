using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsVisual : MonoBehaviour
{
    [SerializeField] private GameObject Border;
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private List<TextMeshPro> pointsText;

    private void Start()
    {
        HideBorder();
    }

    public void SetPlayerName(string playerName)
    {
        playerNameText.text = playerName;
    }

    public void SetPoints(ResourceType tileType, int points)
    {
        pointsText[(int)tileType].text = points.ToString();
    }

    public void ShowBorder()
    {
        //Border.SetActive(true);
    }

    public void HideBorder()
    {
        //Border.SetActive(false);
    }
}