using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button continueButton;

    private bool isPaused = false;

    void Start()
    {
        Debug.Log("Start() PauseManager");
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(PauseGame);
        continueButton.onClick.AddListener(ResumeGame);
    }

    public void PauseGame()
    {
        Debug.Log("PauseGame() PauseManager");
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Debug.Log("Resume() PauseManager");
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
}
