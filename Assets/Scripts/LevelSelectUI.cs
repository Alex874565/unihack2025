using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private Button lvl1Button;

    private void Awake()
    {
        lvl1Button.onClick.AddListener(() =>
        {
            Debug.Log("Loading Level 1");
            SceneManager.LoadScene(1);
        });
    }
}
