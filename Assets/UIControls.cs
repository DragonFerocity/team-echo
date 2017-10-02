using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class UIControls : MonoBehaviour {

    public Canvas inGameMenu;

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Use this for initialization
    void Start () {
        inGameMenu = inGameMenu.GetComponent<Canvas>();
        inGameMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Main Menu"))
        {
            inGameMenu.enabled = !inGameMenu.enabled;

        }


    }
}
