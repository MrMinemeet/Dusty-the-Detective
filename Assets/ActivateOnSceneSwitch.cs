using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnSceneSwitch : MonoBehaviour
{
    public GameObject dirtPointer;
    public GameObject dirtDist;
    void Update()
    {
        dirtDist.SetActive(Globals.AllowDirtPlacement);
        dirtPointer.SetActive(Globals.AllowDirtPlacement);
    }

}