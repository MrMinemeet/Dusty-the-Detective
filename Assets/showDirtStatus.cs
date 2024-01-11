using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showDirtStatus : MonoBehaviour
{
    public Animator animator;

    public GameObject vomit;
    public GameObject wine;
    public GameObject glue;

    private Image vomitImage;
    private Image wineImage;
    private Image glueImage;
    
    // Update is called once per frame
    void Update()
    {
        switch(Globals.vomitStatus) 
        {
            case TrashStatus.ACTIVE:
                vomitImage = vomit.GetComponent<Image>();
                vomitImage.color = new Color(vomitImage.color.r, vomitImage.color.g, vomitImage.color.b, 0.1f); 
                break;
            case TrashStatus.COLLECTED:
                vomitImage = vomit.GetComponent<Image>();
                vomitImage.color = new Color(vomitImage.color.r, vomitImage.color.g, vomitImage.color.b, 1f);
                break;
            case TrashStatus.DISPOSED:
                vomitImage = vomit.GetComponent<Image>();
                vomitImage.color = new Color(vomitImage.color.r, vomitImage.color.g, vomitImage.color.b, 0f);
                break;
        }
        
        switch(Globals.glueStatus) 
        {
            case TrashStatus.ACTIVE:
                glueImage = glue.GetComponent<Image>();
                glueImage.color = new Color(glueImage.color.r, glueImage.color.g, glueImage.color.b, 0.1f); 
                break;
            case TrashStatus.COLLECTED:
                glueImage = glue.GetComponent<Image>();
                glueImage.color = new Color(glueImage.color.r, glueImage.color.g, glueImage.color.b, 1f);
                break;
            case TrashStatus.DISPOSED:
                glueImage = glue.GetComponent<Image>();
                glueImage.color = new Color(glueImage.color.r, glueImage.color.g, glueImage.color.b, 0f);
                break;
        }
        
        switch(Globals.wineStatus) 
        {
            case TrashStatus.ACTIVE:
                wineImage = wine.GetComponent<Image>();
                wineImage.color = new Color(wineImage.color.r, wineImage.color.g, wineImage.color.b, 0.1f); 
                break;
            case TrashStatus.COLLECTED:
                wineImage = wine.GetComponent<Image>();
                wineImage.color = new Color(wineImage.color.r, wineImage.color.g, wineImage.color.b, 1f);
                break;
            case TrashStatus.DISPOSED:
                wineImage = wine.GetComponent<Image>();
                wineImage.color = new Color(wineImage.color.r, wineImage.color.g, wineImage.color.b, 0f);
                break;
        }
    }
}
