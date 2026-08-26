using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Pannello di selezione livelli: sblocca/blocca i pulsanti e mostra il lucchetto
// sui livelli non ancora disponibili. Per ora lo stato sbloccato/bloccato di ogni
// livello si imposta a mano qui sotto (checkbox "Unlocked" in Inspector); il
// salvataggio persistente dei progressi arriverà con la Milestone 10.
public sealed class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
    private sealed class LevelSlot
    {
        public Button button;
        public GameObject lockIcon;

        [Tooltip("Nome della scena da caricare (deve essere aggiunta a File > Build Settings).")]
        public string sceneName;

        public bool unlocked;
    }

    [SerializeField] private LevelSlot[] levels = new LevelSlot[3];

    private void Awake()
    {
        foreach (LevelSlot level in levels)
        {
            if (level.button == null)
            {
                continue;
            }

            level.button.interactable = level.unlocked;

            if (level.lockIcon != null)
            {
                level.lockIcon.SetActive(!level.unlocked);
            }

            if (level.unlocked)
            {
                string sceneName = level.sceneName;
                level.button.onClick.AddListener(() => LoadLevel(sceneName));
            }
        }
    }

    private void LoadLevel(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("LevelSelectManager: nessuna scena assegnata a questo livello.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
