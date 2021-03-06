using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

namespace Echo.Entity
{
    [RequireComponent(typeof(PlayerEntity))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public static PlayerEntity m_Character;
        private bool m_Jump;
        private bool dashRight;
        private bool dashLeft;
        private bool crouch;


        private void Awake()
        {
            m_Character = GetComponent<PlayerEntity>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                // Read when the jump input is released.
                //m_Jump = CrossPlatformInputManager.GetButtonUp("Jump");
            }
            
            /*
            if (CrossPlatformInputManager.GetButton("Main Menu") & UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DefaultAssets")
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                SceneManager.LoadScene("Assets/MainMenu.unity", LoadSceneMode.Single);
                Debug.Log("Active scene is '" + scene.name + "'.");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            if (CrossPlatformInputManager.GetButton("Main Menu") & UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MainMenu")
            {
                Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                SceneManager.LoadScene("Assets/DefaultAssets.unity", LoadSceneMode.Single);
                Debug.Log("Active scene is '" + scene.name + "'.");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu.unity", UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            */

        }
        


    private void FixedUpdate()
    {
      // Read the inputs.
      dashRight = CrossPlatformInputManager.GetButtonDown("DashRight");
      dashLeft = CrossPlatformInputManager.GetButtonDown("DashLeft");
      crouch = CrossPlatformInputManager.GetButton("Crouch");
      float h = CrossPlatformInputManager.GetAxis("Horizontal");
      // Pass all parameters to the character control script.
      m_Character.Move(h * m_Character.m_RunSpeed, crouch, m_Jump, dashRight, dashLeft);
      m_Jump = false;
    }
  }
}
