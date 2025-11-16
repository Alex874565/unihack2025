using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private GameObject levelSelector;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            //SceneManager.LoadScene(1);
            levelSelector.SetActive(true);
            gameObject.SetActive(false);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
        quitButton.onClick.AddListener(() =>
        {
            Debug.Log("quitting");
            Application.Quit();
        });
    }
}
