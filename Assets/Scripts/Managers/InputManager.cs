using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ServiceLocator.Instance.ShopUIManager.ShowShop();
        }
    }
}
