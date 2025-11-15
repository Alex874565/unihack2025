using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private ShopUIManager shopUIManager; // assign in inspector

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            shopUIManager.HideShop(); // <-- calls proper hide and resumes game
        });
    }

    [SerializeField] private Button closeButton;

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
