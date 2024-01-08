using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public void PlayGame()
    {
        // Load main menu
        StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel("Lobby"));
    }
    
    public void BackToMainMenu()
    {
        // Load main menu
        StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel("MainScreen"));
    }

    public void GoToControls()
    {
        // Load main menu
        StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel("Controls"));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Won't do anything in editor
    }
}
