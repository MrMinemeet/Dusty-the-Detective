using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public static bool IsActive;
    
    public TextMeshProUGUI vomitResult;
    public TextMeshProUGUI wineResult;
    public TextMeshProUGUI glueResult;
    
    public void Setup(bool vomit, bool wine, bool glue)
    {
        IsActive = true;
        gameObject.SetActive(true);
        vomitResult.text = vomit ? "CORRECT" : "INCORRECT";
        wineResult.text = wine ? "CORRECT" : "INCORRECT";
        glueResult.text = glue ? "CORRECT" : "INCORRECT";
    }

    public void OnMainMenuClick()
    {
        // Load main menu
        StartCoroutine(FindObjectOfType<LevelLoader>().LoadLevel("MainScreen"));
    }
}
