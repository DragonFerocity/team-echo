using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Echo.Entity
{
  [RequireComponent(typeof(EnemyEntity))]
  public class Platformer2DEnemyControl : MonoBehaviour
  {
    private EnemyEntity m_Character;
    private bool m_Jump;


    private void Awake()
    {
      m_Character = GetComponent<EnemyEntity>();
    }


    private void Update()
    {
      if (!m_Jump)
      {
        // Read the jump input in Update so button presses aren't missed.
        //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        // Read when the jump input is released.
        //m_Jump = CrossPlatformInputManager.GetButtonUp("Jump");
      }
    }


    private void FixedUpdate()
    {
      // Read the inputs.
      //bool crouch = Input.GetKey(KeyCode.LeftControl);
      //float h = CrossPlatformInputManager.GetAxis("Horizontal");
      // Pass all parameters to the character control script.
      //m_Character.Move(h, crouch, m_Jump);
      //m_Jump = false;
    }
  }
}
