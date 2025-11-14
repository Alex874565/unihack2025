using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGameButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            PauseManager.Instance.ResumeGame();
        });
        closeButton.onClick.AddListener(() =>
        {
            PauseManager.Instance.ResumeGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
        exitGameButton.onClick.AddListener(() =>
        {
            Debug.Log("quitting");
            Application.Quit();
        });
    }
    private void Start()
    {
        PauseManager.Instance.OnGamePaused += Instance_OnGamePaused;
        PauseManager.Instance.OnGameUnpaused += Instance_OnGameUnpaused;
        
        Hide();
    }

    private void Instance_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Instance_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
