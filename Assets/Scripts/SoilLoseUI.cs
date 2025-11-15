using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoilLoseUI : MonoBehaviour
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
