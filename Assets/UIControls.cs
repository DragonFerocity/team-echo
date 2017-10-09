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
