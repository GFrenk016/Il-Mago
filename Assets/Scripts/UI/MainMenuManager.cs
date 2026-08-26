using UnityEngine;

// Menu principale del gioco: gestisce il passaggio tra il pannello Menu (Gioca/Esci)
// e il pannello di selezione livelli, oltre all'uscita dall'applicazione.
// Va messo su un GameObject "manager" nella scena MainMenu (es. il GameObject "UI").
public sealed class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject levelSelectPanel;

    private void Awake()
    {
        ShowMainPanel();
    }

    // Da collegare all'OnClick del pulsante "Gioca".
    public void ShowLevelSelect()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(false);
        }

        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    // Da collegare all'OnClick del pulsante "Indietro" nel pannello di selezione livelli.
    public void ShowMainPanel()
    {
        if (mainPanel != null)
        {
            mainPanel.SetActive(true);
        }

        if (levelSelectPanel != null)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    // Da collegare all'OnClick del pulsante "Esci".
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
