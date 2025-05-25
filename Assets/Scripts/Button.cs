using Reflex.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class Button : MonoBehaviour
{
    [Inject] private readonly Players players;

    [SerializeField] private Color enableColor = Color.white;
    [SerializeField] private Color disableColor = Color.gray;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color clickColor = Color.cyan;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textName;
    [SerializeField] private string actionName;
    [SerializeField] private ActionType actionType;
    [SerializeField] private List<Sprite> spriteIconPrefabs;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float clickFlashDuration = 0.1f;
    [SerializeField] private List<int> costs;
    [SerializeField] private List<TextMeshPro> textCost;
    [SerializeField] private List<SpriteRenderer> spriteIconCost;
    [SerializeField] private TextMeshPro textIncome;
    [SerializeField] private SpriteRenderer spriteIconIncome;
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int income;

    private bool isHovered = false;
    private Coroutine clickRoutine;

    private void Awake()
    {
        UpdateVisual();
        SetupVisual();
    }

    private void OnMouseEnter()
    {
        isHovered = true;
        UpdateVisual();
    }

    private void OnMouseExit()
    {
        isHovered = false;
        UpdateVisual();
    }

    private void OnMouseDown()
    {
        if (!isEnabled)
        {
            return;
        }

        if (clickRoutine != null)
        {
            StopCoroutine(clickRoutine);
        }

        clickRoutine = StartCoroutine(ClickFlash());
        players.CurrentPlayer.ExecuteAction(actionType, costs);
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        UpdateVisual();
    }

    public int GetCost(int index)
    {
        if (index < 0 || index >= costs.Count)
        {
            return 0;
        }
        return costs[index];
    }

    public ActionType GetActionType()
    {
        return actionType;
    }

    private void UpdateVisual()
    {
        if (!isEnabled)
        {
            spriteRenderer.color = disableColor;
        }
        else if (isHovered)
        {
            spriteRenderer.color = hoverColor;
        }
        else
        {
            spriteRenderer.color = enableColor;
        }
    }

    private void SetupVisual()
    {
        textName.text = actionName;

        textIncome.text = $"+{income}";
        spriteIconIncome.sprite = spriteIconPrefabs[(int)resourceType];

        int textNumber = 0;

        for (int i = 0; i < 8; i++)
        {
            if (costs[i] == 0)
            {
                continue;
            }

            textCost[textNumber].text = $"-{costs[i]}";
            spriteIconCost[textNumber].gameObject.transform.parent.gameObject.SetActive(true);
            spriteIconCost[textNumber].sprite = spriteIconPrefabs[i];

            if (textNumber == 2)
            {
                break;
            }
            textNumber++;
        }
    }

    private IEnumerator ClickFlash()
    {
        spriteRenderer.color = clickColor;
        yield return new WaitForSeconds(clickFlashDuration);
        UpdateVisual();
    }
}