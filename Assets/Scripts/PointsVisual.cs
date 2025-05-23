using TMPro;
using UnityEngine;

public class PointsVisual : MonoBehaviour
{
    [SerializeField] private GameObject Border;
    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private TextMeshPro points1;
    [SerializeField] private TextMeshPro points2;
    [SerializeField] private TextMeshPro points3;
    [SerializeField] private TextMeshPro points4;
    [SerializeField] private TextMeshPro points5;

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
        switch (tileType)
        {
            case ResourceType.type1:
                points1.text = points.ToString();
                break;

            case ResourceType.type2:
                points2.text = points.ToString();
                break;

            case ResourceType.type3:
                points3.text = points.ToString();
                break;

            case ResourceType.type4:
                points4.text = points.ToString();
                break;

            case ResourceType.type5:
                points5.text = points.ToString();
                break;
        }
    }

    public void ShowBorder()
    {
        Border.SetActive(true);
    }

    public void HideBorder()
    {
        Border.SetActive(false);
    }
}