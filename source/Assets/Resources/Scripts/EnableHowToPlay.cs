using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHowToPlay : MonoBehaviour
{
    public GameObject HowToPlayScreen;
    public GameObject Dusty;
    public GameObject Controls;
    public GameObject Play;
    public GameObject Exit;
    public GameObject GameName;
    

    // Update is called once per frame
    void Update()
    {
        HowToPlayScreen.SetActive(Globals.enableHowToPlay);
        Dusty.SetActive(!Globals.enableHowToPlay);
        Controls.SetActive(!Globals.enableHowToPlay);
        Play.SetActive(!Globals.enableHowToPlay);
        Exit.SetActive(!Globals.enableHowToPlay);
        GameName.SetActive(!Globals.enableHowToPlay);
    }

    public void enableHowToPlay()
    {
        Globals.enableHowToPlay = true;
    }
}
