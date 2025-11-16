using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BoosterIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI stackText;

    [Header("Description Tooltip")]
    [SerializeField] private GameObject descriptionContainer;   // the parent GameObject
    [SerializeField] private TextMeshProUGUI descriptionText;   // text component
    [SerializeField] private Image descriptionBackground;       // optional background

    private float duration;
    public GlobalModifierData Modifier { get; private set; }

    private void Awake()
    {
        if (descriptionContainer != null)
            descriptionContainer.SetActive(false); // hide tooltip initially
    }

    public void Initialize(GlobalModifierData modifier, float duration, int stack = 1)
    {
        Debug.Log("Initializing " + modifier.Description);
        Modifier = modifier;
        this.duration = duration;

        iconImage.sprite = modifier.Icon;
        timerImage.sprite = modifier.Icon;
        timerImage.fillAmount = 1f;

        SetStack(stack);

        if (descriptionText != null)
            descriptionText.text = modifier.Description;
    }

    public void UpdateTimer(float remaining)
    {
        if (duration <= 0f) return;
        timerImage.fillAmount = Mathf.Clamp01(remaining / duration);
    }

    public void SetStack(int count)
    {
        if (stackText != null)
            stackText.text = count > 1 ? count.ToString() : "";
    }

    // Show description when hovering
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionContainer != null)
            descriptionContainer.SetActive(true);
    }

    // Hide description when pointer leaves
    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionContainer != null)
            descriptionContainer.SetActive(false);
    }
}
