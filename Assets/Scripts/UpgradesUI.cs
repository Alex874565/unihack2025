using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] private ShopUIManager shopUIManager; // assign in inspector
    [SerializeField] private Button closeButton;

    private void Awake()
{
    Debug.Log("closeButton = " + closeButton);
    Debug.Log("shopUIManager = " + shopUIManager);

    closeButton.onClick.AddListener(() =>
    {
        shopUIManager.HideShop();
    });
}


    

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
