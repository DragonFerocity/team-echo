using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echo.Entity
{
  public abstract class Entity : MonoBehaviour, IEntity
  {
    protected int m_MaxHealth;

    [SerializeField] protected LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    [SerializeField] protected bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [Range(0, 1)] [SerializeField] protected float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] public float m_RunSpeed = 10f;
    [SerializeField] public float m_WalkSpeed = 5f;


    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float k_WallCheckRadius = 0.6f; // Radius of the overlap circle to determine if wall is touching the player
    protected Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    public Rigidbody2D m_Rigidbody2D;
    public Renderer m_Renderer2D;
    protected Animator m_Anim; // Reference to the player's animator component.
    protected Transform m_CeilingCheck;   // A position marking where to check for ceilings
    protected const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up

    protected bool m_Grounded;            // Whether or not the player is grounded.
    protected bool m_FacingRight = true;  // For determining which way the player is currently facing.
    protected bool m_Dashing = false;     // True if the player has initiated a dash
    [Range(0, 5000)] [SerializeField] protected float m_JumpForce = 800; //Acceleration of the object in m/s^2 (jump)
    [Range(0, 5000)] [SerializeField] protected float m_DashForce = 800; //Acceleration of the object in m/s^2 (dash)


    public bool crouch
    {
      get
      {
        return m_Anim.GetBool("Crouch");
      }
      set
      {
        m_Anim.SetBool("Crouch", value);
      }
    }
    public Direction facing
    {
      get
      {
        if (m_FacingRight)
          return Direction.RIGHT;
        else
          return Direction.LEFT;
      }
      set
      {
        Vector3 theScale = transform.localScale;

        // Switch the way the player is labelled as facing.
        if (value == Direction.RIGHT)
        {
          m_FacingRight = true;
          theScale.x = 1;
        }
        else
        {
          m_FacingRight = false;
          theScale.x = -1;
        }

        transform.localScale = theScale;
      }
    }

    public virtual void Awake()
    {
      // Setting up references.
      m_Rigidbody2D = GetComponent<Rigidbody2D>();
      m_Renderer2D = GetComponent<Renderer>();
      m_Anim = GetComponent<Animator>();
      m_GroundCheck = transform.Find("GroundCheck");
      m_CeilingCheck = transform.Find("CeilingCheck");
    }
    public abstract void Start();
    public void FixedUpdate()
    {
      m_Grounded = false;
      // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
      // This can be done using layers instead but Sample Assets will not overwrite your project settings.
      Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
      for (int i = 0; i < colliders.Length; i++)
      {
        if (colliders[i].gameObject != gameObject)
        {
          m_Grounded = true;
        }
      }
      m_Anim.SetBool("Ground", m_Grounded);

      // Set the vertical animation
      m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }

    public void Move(float move, bool _crouch, bool jump, bool dash_right, bool walk = false)
    {
      if (walk)
        m_Anim.speed = m_WalkSpeed / m_RunSpeed;
      // If crouching, check to see if the character can stand up
      if (!_crouch && m_Anim.GetBool("Crouch"))
      {
        // If the character has a ceiling preventing them from standing up, keep them crouching
        if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        {
          _crouch = true;
        }
      }

      // Set whether or not the character is crouching in the animator
      this.crouch = _crouch;

      //only control the player if grounded or airControl is turned on
      if (m_Grounded || m_AirControl)
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

    public void moveLeft()
    {
      m_Rigidbody2D.velocity = new Vector2(-m_RunSpeed, m_Rigidbody2D.velocity.y);
    }
    public void moveRight()
    {
      m_Rigidbody2D.velocity = new Vector2(m_RunSpeed, m_Rigidbody2D.velocity.y);
    }

    public Direction getDirectionToEntity(Entity otherEntity)
    {
      Vector2 otherEntityLocation = otherEntity.m_Rigidbody2D.position;
      Vector2 thisLocation = m_Rigidbody2D.position;

      if (otherEntityLocation.y > thisLocation.y - 275)
        return Direction.UP;
      else if (otherEntityLocation.x > thisLocation.x + 275)
        return Direction.RIGHT;
      else if (otherEntityLocation.y > thisLocation.y + 275)
        return Direction.DOWN;
      else if (otherEntityLocation.x > thisLocation.x - 275)
        return Direction.LEFT;
      else
        return Direction.NULL;
    }

    public SByte getHorizontalDirectionToEntity(Entity otherEntity)
    {
      Vector2 otherEntityLocation = otherEntity.m_Rigidbody2D.position;
      Vector2 thisLocation = m_Rigidbody2D.position;

      if (otherEntityLocation.x > thisLocation.x)
        return 1;
      else if (otherEntityLocation.x < thisLocation.x)
        return -1;
      else
        return 0;
    }

    public Vector2 getDistanceToEntity(Entity otherEntity)
    {
      Vector2 otherEntityLocation = otherEntity.m_Rigidbody2D.position;
      Vector2 thisLocation = m_Rigidbody2D.position;

      return new Vector2(otherEntityLocation.x - thisLocation.x, otherEntityLocation.y - thisLocation.y);
    }
    public float getAbsoluteDistanceToEntity(Entity otherEntity)
    {
      return getDistanceToEntity(otherEntity).magnitude;
    }

    public abstract void Attack();

    public int GiveMillionDollars()
    {
      return 1000000;
    }

    public void Jump(float multiplier = 1)
    {
      m_Anim.SetBool("Ground", false);
      m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * multiplier));
    }

    //Dash right
    /*public void DashRight()
    {
      m_Rigidbody2D.AddForce(new Vector2(m_DashForce, 0));
    }*/

    public virtual void Flip()
    {
      // Switch the way the player is labelled as facing.
       m_FacingRight = !m_FacingRight;

      // Multiply the player's x local scale by -1.
      Vector3 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
    }

    protected virtual bool isGroundInFrontOf(float distance = 0.5f)
    {
      if (facing == Direction.LEFT)
      {
        Vector3 newPos = new Vector3(m_GroundCheck.position.x - distance, m_GroundCheck.position.y, m_GroundCheck.position.z);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPos, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
          if (colliders[i].gameObject != gameObject)
          {
            return true;
          }
        }
      }
      else if (facing == Direction.RIGHT)
      {
        Vector3 newPos = new Vector3(m_GroundCheck.position.x + distance, m_GroundCheck.position.y, m_GroundCheck.position.z);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPos, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
          if (colliders[i].gameObject != gameObject)
          {
            return true;
          }
        }
      }
      return false;
    }
    protected virtual bool isWallInFrontOf(float distance = 0.5f)
    {
      if (facing == Direction.LEFT)
      {
        Vector3 newPos = new Vector3(m_Rigidbody2D.position.x - distance, m_Rigidbody2D.position.y, 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPos, k_WallCheckRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
          if (colliders[i].gameObject != gameObject)
          {
            return true;
          }
        }
      }
      else if (facing == Direction.RIGHT)
      {
        Vector3 newPos = new Vector3(m_Rigidbody2D.position.x + distance, m_Rigidbody2D.position.y, 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPos, k_WallCheckRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
          if (colliders[i].gameObject != gameObject)
          {
            return true;
          }
        }
      }
      return false;
    }

    public enum Direction : byte
    {
      NULL = 0,
      UP,
      RIGHT,
      DOWN,
      LEFT
    }
  }
}