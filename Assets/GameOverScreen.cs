using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI vomitResult;
    public TextMeshProUGUI wineResult;
    public TextMeshProUGUI glueResult;
    
    public void Setup(bool vomit, bool wine, bool glue)
    {
        gameObject.SetActive(true);
        if (vomit)
        {
            vomitResult.text = "TRUE";
        }
        else
        {
            vomitResult.text = "FALSE";
        }
        
        if (wine)
        {
            wineResult.text = "TRUE";
        }
        else
        {
            wineResult.text = "FALSE";
        }

        if (glue)
        {
            glueResult.text = "TRUE";
        }
        else
        {
            glueResult.text = "FALSE";
        }
    }
}
