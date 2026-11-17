using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private BodyPartQTEManager bodyPartQTEManager;
    [SerializeField] private FaceQTEManager faceQTEManager;
    [SerializeField] private BodyQTEManager bodyQTEManager;
    [SerializeField] private GameObject mainMenuContent;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;

    private bool isPaused = false;

    private void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
        pauseMenuPanel.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        AudioListener.volume = savedVolume;
        volumeSlider.SetValueWithoutNotify(savedVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
                return;
            }
            if (dialogueManager != null && dialogueManager.IsDialogueOpen)
                return;

            if ((bodyPartQTEManager != null && bodyPartQTEManager.IsQTEActive) ||
                (faceQTEManager != null && faceQTEManager.IsQTEActive) ||
                (bodyQTEManager != null && bodyQTEManager.IsQTEActive))
            {
                return;
            }

            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        mainMenuContent.SetActive(true);
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        mainMenuContent.SetActive(true);
        optionsPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quitting game...");

        Application.Quit();
    }
    public void OpenOptions()
    {
        mainMenuContent.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuContent.SetActive(true);
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
}
