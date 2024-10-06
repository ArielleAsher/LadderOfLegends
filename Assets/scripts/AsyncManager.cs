using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncManager : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;

    [Header("Loading Time")]
    [SerializeField] private float minimumLoadingTime = 3.0f; // Minimum time the loading screen is displayed

    private GameObject gamePanel; // Reference to the game panel (dynamically assigned)

    private bool isGamePanelActive = false; // Track if the game panel is active

    private void Start()
    {
        // Play main menu music on start
        PlayMainMenuMusic();

        // Find and set the game panel reference
        FindGamePanel();
    }

    private void PlayMainMenuMusic()
    {
        // Find the AudioManager instance in the 3rdVersion scene and play main menu music
        GameObject audioManagerObject = GameObject.Find("AudioManager");

        if (audioManagerObject != null)
        {
            AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
            if (audioManager != null)
            {
                audioManager.PlayBGM(audioManager.bgmMainMenu);
            }
            else
            {
                Debug.LogWarning("AudioManager component not found on AudioManager GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager GameObject not found in the scene.");
        }
    }

    public void LoadLevelBtn(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelWithMinTime(levelToLoad));
    }

    private IEnumerator LoadLevelWithMinTime(string levelToLoad)
    {
        float startTime = Time.time;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false; // Prevent the scene from activating immediately

        while (!loadOperation.isDone)
        {
            float elapsedTime = Time.time - startTime;
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = Mathf.Clamp01(elapsedTime / minimumLoadingTime * 0.5f + progressValue * 0.5f);

            if (elapsedTime >= minimumLoadingTime && loadOperation.progress >= 0.9f)
            {
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Hide the loading screen after loading is complete
        loadingScreen.SetActive(false);
    }

    public void OpenGamePanel()
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(true);
            isGamePanelActive = true;
            PlayGameplayMusic();
        }
        else
        {
            Debug.LogWarning("Game panel reference is not set.");
        }
    }

    public void CloseGamePanel()
    {
        if (gamePanel != null)
        {
            gamePanel.SetActive(false);
            isGamePanelActive = false;
            PlayMainMenuMusic();
        }
        else
        {
            Debug.LogWarning("Game panel reference is not set.");
        }
    }

    private void PlayGameplayMusic()
    {
        // Find the AudioManager instance in the 3rdVersion scene and play gameplay music
        GameObject audioManagerObject = GameObject.Find("AudioManager");

        if (audioManagerObject != null)
        {
            AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
            if (audioManager != null)
            {
                audioManager.PlayBGM(audioManager.bgmGameplay);
            }
            else
            {
                Debug.LogWarning("AudioManager component not found on AudioManager GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager GameObject not found in the scene.");
        }
    }

    private void FindGamePanel()
    {
        // Attempt to find the game panel in the scene hierarchy
        GameObject foundGamePanel = GameObject.Find("GamePanel");

        if (foundGamePanel != null)
        {
            gamePanel = foundGamePanel;
        }
        else
        {
            Debug.LogWarning("Game panel not found in the scene hierarchy.");
        }
    }
}
