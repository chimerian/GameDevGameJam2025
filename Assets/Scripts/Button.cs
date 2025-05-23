using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Color enableColor = Color.white;
    [SerializeField] private Color disableColor = Color.gray;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color clickColor = Color.cyan;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float clickFlashDuration = 0.1f;

    private bool isHovered = false;
    private Coroutine clickRoutine;

    private void Awake()
    {
        UpdateVisual();
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
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        UpdateVisual();
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

    private IEnumerator ClickFlash()
    {
        spriteRenderer.color = clickColor;
        yield return new WaitForSeconds(clickFlashDuration);
        UpdateVisual();
    }
}