using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance {get; private set;}

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private PlayerInputActions inputActions;
    private bool isPaused = false;

    private void Awake()
    {
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }

    Instance = this;

    inputActions = new PlayerInputActions();
    }


    private void OnEnable()
    {
        inputActions.Player.Pause.performed += OnPause;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnPause;
        inputActions.Player.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
{
    Time.timeScale = 0f; // Stop game time
    OnGamePaused?.Invoke(this, EventArgs.Empty);
    isPaused = true;
    Debug.Log("Game Paused");
}


    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume game
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        isPaused = false;
        Debug.Log("Game Resumed");
        // disable pause UI here if you want
    }
}
