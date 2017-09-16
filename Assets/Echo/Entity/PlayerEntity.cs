using System;
using UnityEngine;

namespace Echo.Entity
{
  public class PlayerEntity : Entity
  {
    //[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.

    private bool m_DoubleJump = false;  // If true, the player has touched the ground and air jump is reset
        private bool m_FromGround = false;  //If true, the player has jumped from the ground
    private short m_DoubleJumpWait = 0;
    private System.Diagnostics.Stopwatch m_DoubleJumpWaitTimer = new System.Diagnostics.Stopwatch();

        private bool m_CanDash = true;
    private System.Diagnostics.Stopwatch m_DashWaitTimer = new System.Diagnostics.Stopwatch();

        public new void Awake()
    {
      base.Awake();
    } 

    public override void Start()
    {
      //throw new NotImplementedException();
    }

    public void Update()
    {
      //throw new NotImplementedException();
    }

    public new void FixedUpdate()
    {
      base.FixedUpdate();
    }


    public new void Move(float move, bool crouch, bool jump, bool dash_right)
    {
      base.Move(move, crouch, jump, dash_right);

      //Reset air jump if player touches ground
      if (m_Grounded)
       {
        m_DoubleJump = true;
       }

      //Reset dash if it has been 150 ms since last dash
      if (!m_CanDash && m_DashWaitTimer.ElapsedMilliseconds <= 150)
      {
                m_Rigidbody2D.velocity = new Vector2(25f, 0);
      }
      else if (m_DashWaitTimer.ElapsedMilliseconds >= 600)
      {
                m_CanDash = true;
                m_DashWaitTimer.Stop();
                m_DashWaitTimer.Reset();
      }

      // If the player should jump...
      if (m_Grounded && jump && m_Anim.GetBool("Ground"))
      {
        // Add a vertical force to the player.
        m_Grounded = false;
        Jump();
        m_DoubleJumpWaitTimer.Start();
                Debug.Log("Only once?");
      }
      //Allow for a second jump
      else if (m_DoubleJump && jump && !m_Grounded)
      {
        if (m_FromGround && m_DoubleJumpWaitTimer.ElapsedMilliseconds <= 150)
        {
            //do nothing
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            Jump();
            m_DoubleJump = false;
            m_FromGround = false;
            m_DoubleJumpWait = 0;
            m_DoubleJumpWaitTimer.Stop();
            m_DoubleJumpWaitTimer.Reset();
        }
      }

      // If the player should dash...
      if(dash_right && m_CanDash)
      {
        m_CanDash = false;
        m_DashWaitTimer.Start();
        //DashRight();
      }
    }

    public override void Attack()
    {
      //throw new NotImplementedException();
    }

        // Used for Item Pickups
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Pickup"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
