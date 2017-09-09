using System;
using UnityEngine;

namespace Echo.Entity
{
  public class PlayerEntity : Entity
  {
    [SerializeField] protected new int m_MaxHealth = 200;
    [SerializeField] protected new float m_WalkSpeed = 10f;                    // The fastest the player can travel in the x axis.
    //[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.

    private bool m_DoubleJump = false;  // If true, the player has already jumped once without touching the ground
    private short m_DoubleJumpWait = 0;
    private System.Diagnostics.Stopwatch m_DoubleJumpWaitTimer = new System.Diagnostics.Stopwatch();

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


    public new void Move(float move, bool crouch, bool jump)
    {
      base.Move(move, crouch, jump);

      //Reset air jump if player touches ground
      if(m_Grounded)
       {
        m_DoubleJump = true;
       }

      // If the player should jump...
      if (m_Grounded && jump && m_Anim.GetBool("Ground"))
      {
        // Note the first jump is taken
       // m_DoubleJump = true;
        // Add a vertical force to the player.
        m_Grounded = false;
        Jump();
        m_DoubleJumpWaitTimer.Start();
      }
      //Allow for a second jump
      else if (m_DoubleJump && m_DoubleJumpWaitTimer.ElapsedMilliseconds >= 300 && jump && !m_Grounded)
      {
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
        Jump();
        m_DoubleJump = false;
        m_DoubleJumpWait = 0;
        m_DoubleJumpWaitTimer.Stop();
        m_DoubleJumpWaitTimer.Reset();
      }
    }

    public override void attack()
    {
      //throw new NotImplementedException();
    }
  }
}
