using UnityEngine;
using UnityEngine.SceneManagement;

// Schermata di Game Over. Va messo su un GameObject "manager" nella scena (o sul
// Canvas stesso). Il pannello si assegna una sola volta nell'Inspector; il pulsante
// "Riprova" chiama RestartLevel() dal suo OnClick.
public sealed class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private bool pauseGameplayOnDeath = true;

    public static GameOverUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Show()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (pauseGameplayOnDeath)
        {
            Time.timeScale = 0f;
        }
    }

    // Da collegare all'OnClick del pulsante "Riprova".
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
