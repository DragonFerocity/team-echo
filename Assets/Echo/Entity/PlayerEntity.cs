using System;
using UnityEngine;

namespace Echo.Entity
{
    public class PlayerEntity : Entity
    {
        //[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.

        private bool m_DoubleJump = false;  // If true, the player has touched the ground and air jump is reset
        private bool m_FromGround = false;  //If true, the player has jumped from the ground
        private bool m_WallHit = false;
        private short m_DoubleJumpWait = 0;
        private System.Diagnostics.Stopwatch m_DoubleJumpWaitTimer = new System.Diagnostics.Stopwatch();

        //Dash control variables
        private bool m_DashingRight = false;
        private bool m_DashingLeft = false;
        private bool m_EndDash = true; //Keep track of if the dash motion has finished (not the wait time)
        private System.Diagnostics.Stopwatch m_DashWaitTimer = new System.Diagnostics.Stopwatch();

        //Wall jump control variables
        private bool m_WallJumpRight = false; //Hit a wall while traveling left, therefore jump right
        private bool m_WallJumpLeft = false; //Hit a wall while traveling right, therefore jump left
        private System.Diagnostics.Stopwatch m_WallJumpTimer = new System.Diagnostics.Stopwatch();
        public float distance = 5f;
        private float HorizontalWallVelocity = 0;


        const float K_BoxWidth = 0.2f;
        ContactPoint2D[] contacts = new ContactPoint2D[10];

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

            Physics2D.queriesStartInColliders = false;

        }

        public new void Move(float move, bool crouch, bool jump, bool dash_right, bool dash_left)
        {
            base.Move(move, crouch, jump, dash_right);

            RaycastHit2D wallHit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);


            if (jump && !m_Grounded && (wallHit.collider != null))
            {
                HorizontalWallVelocity = 25f * wallHit.normal.x;
                m_Rigidbody2D.velocity = new Vector2(10 * wallHit.normal.x, 10);
                m_WallJumpTimer.Start();
                if (!m_FacingRight)
                {
                    m_WallJumpRight = true;
                }
                else
                {
                    m_WallJumpLeft = true;
                }
                Flip();
            }

            if (m_WallJumpTimer.ElapsedMilliseconds <= 250 && m_WallJumpRight && m_FacingRight)
            {
                m_Rigidbody2D.velocity = new Vector2(15f, (250 - m_WallJumpTimer.ElapsedMilliseconds) / 10);
            }
            else if (m_WallJumpTimer.ElapsedMilliseconds <= 250 && m_WallJumpLeft && !m_FacingRight)
            {
                m_Rigidbody2D.velocity = new Vector2(-15f, (250 - m_WallJumpTimer.ElapsedMilliseconds) / 10);
            }
            else
            {
                m_WallJumpTimer.Stop();
                m_WallJumpTimer.Reset();
                m_WallJumpRight = false;
                m_WallJumpLeft = false;
            }


            if (m_Grounded)
            {
                m_DoubleJump = true;
                m_FromGround = true;
            }
            else
            {
                m_FromGround = false;
            }

            //Right and left dash handling
            //Reset dash if it has been 150 ms since last dash
            if (m_DashingRight && m_DashWaitTimer.ElapsedMilliseconds <= 150)
            {
                m_Rigidbody2D.velocity = new Vector2(25f, 0);
            }
            else if (m_DashingLeft && m_DashWaitTimer.ElapsedMilliseconds <= 150)
            {
                m_Rigidbody2D.velocity = new Vector2(-25f, 0);
            }
            else if ((m_DashWaitTimer.ElapsedMilliseconds >= 450) || m_Grounded)
            {
                m_DashingRight = false;
                m_DashingLeft = false;
                m_DashWaitTimer.Stop();
                m_DashWaitTimer.Reset();
            }
            else if ((m_DashWaitTimer.ElapsedMilliseconds > 150) && !m_EndDash)
            {
                m_Rigidbody2D.velocity = new Vector2(0, 0);
                m_EndDash = true;
            }

            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                Jump();
                m_DoubleJumpWaitTimer.Start();
            }
            //Allow for a second jump
            else if (m_DoubleJump && jump && !m_Grounded && !m_WallJumpLeft && !m_WallJumpRight && m_EndDash)
            {
                if (m_FromGround && m_DoubleJumpWaitTimer.ElapsedMilliseconds <= 100)
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
            if (dash_right && !m_DashingRight && !m_DashingLeft)
            {
                m_DashingRight = true;
                m_EndDash = false;
                m_DashWaitTimer.Start();
                //If player is not facing the same direction as the dash, flip them
                if (!m_FacingRight)
                {
                    Flip();
                }
            }
            if (dash_left && !m_DashingRight && !m_DashingLeft)
            {
                m_DashingLeft = true;
                m_EndDash = false;
                m_DashWaitTimer.Start();
                if (m_FacingRight)
                {
                    Flip();
                }
            }

            //Basic movement block for PLAYER ENTITY ONLY
            if ((m_Grounded || m_AirControl) && !m_WallJumpLeft && !m_WallJumpRight && !m_DashingRight && !m_DashingLeft)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move * m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Cast a line in front of the player (currently used just for checking for walls)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
        }

    } //End of player entity
}   //End of namespace for entity