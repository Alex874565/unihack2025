using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaterLoseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
