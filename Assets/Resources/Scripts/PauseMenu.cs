using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused;
    [SerializeField] private GameObject _pauseMenuUI;
    
    private void Start()
    {
        if (_pauseMenuUI == null)
        {
            _pauseMenuUI = GameObject.Find("PauseMenu");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (IsGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void ResumeGame()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }
    
    private void PauseGame()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freeze time of game
        IsGamePaused = true;
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu");
        Time.timeScale = 1f;
        IsGamePaused = false;
        
        // Load main menu
        StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel("MainScreen"));
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Won't do anything in editor
    }
}
