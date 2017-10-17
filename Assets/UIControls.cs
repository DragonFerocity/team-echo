using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class UIControls : MonoBehaviour {

    public Canvas inGameMainMenu;
    public Canvas inGameLevelsMenu;

    // Use this for initialization
    void Start()
    {
        inGameMainMenu = inGameMainMenu.GetComponent<Canvas>();
        inGameMainMenu.enabled = false;
        inGameLevelsMenu.enabled = false;
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }


    public void MainMenuToLevelMenu()
    {
        inGameMainMenu.enabled = false;
        inGameLevelsMenu.enabled = true;
    }

    public void LevelMenuToMainMenu()
    {
        inGameLevelsMenu.enabled = false;
        inGameMainMenu.enabled = true;
    }
    //Vector3(-24, -189, 0); level1
    //Vector3(-24, -148, 0); tutorial
    public void TeleportToTutorial(Transform t)
    {
        t.position = new Vector3(-24, -148, 0);
    }
    public void TeleportToTesting(Transform t)
    {
        t.position = new Vector3(83, -107, 0);
    }
    public void TeleportToNexus(Transform t)
    {
        t.position = new Vector3(-24, -107, 0);
    }
    public void TeleportToLevel1(Transform t)
   {
        t.position = new Vector3(-24, -189, 0);
    }
    public void TeleportToLevel2(Transform t)
    {
        t.position = new Vector3(38, -270, 0);
    }
    public void TeleportToLevel3(Transform t)
    {
        t.position = new Vector3(-24, -107, 0);
    }
    public void TeleportToLevel4(Transform t)
    {
        t.position = new Vector3(-24, -107, 0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }




    // Update is called once per frame
    void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Main Menu"))
        {
            inGameMainMenu.enabled = !inGameMainMenu.enabled;
            if (inGameLevelsMenu.enabled == true)
            {
                inGameLevelsMenu.enabled = false;
            }  
        }
    }
}
