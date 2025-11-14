using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P pressed");
            GameManager.Instance.ShopManager.SelectUpgrades();
        }
    }
}
