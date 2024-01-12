using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI vomitResult;
    public TextMeshProUGUI wineResult;
    public TextMeshProUGUI glueResult;
    
    public void Setup(bool vomit, bool wine, bool glue)
    {
        gameObject.SetActive(true);
        vomitResult.text = vomit ? "CORRECT" : "INCORRECT";
        wineResult.text = wine ? "CORRECT" : "INCORRECT";
        glueResult.text = glue ? "CORRECT" : "INCORRECT";
    }
}
