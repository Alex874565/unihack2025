using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S pressed");
            ServiceLocator.Instance.ShopManager.SelectNextItems();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ServiceLocator.Instance.ModulesManager.BuyModule(null);
        }
    }
}
