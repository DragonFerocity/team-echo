﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Echo.Entity
{
  public class EnemyEntity : Entity
  {
    public Point.Point initialPosition;
    [Range(0, 100)] [SerializeField] protected int idleDistance = 6;
    [Range(0, 100)] [SerializeField] protected int idleDistanceVariability = 3;
    [Range(0, 100)] [SerializeField] protected int engageDistance = 6;
    [Range(0, 100)] [SerializeField] protected int disengageDistance = 12;
    protected System.Random randomIdleDist = new System.Random();
    protected int nextRandomDist;
    private float floatY = 0;
    private bool flyingEntity = false; //Used to tell if the entity was enabled as a flying entity from the beginning of the game

    protected System.Diagnostics.Stopwatch IdleStopWatch = new System.Diagnostics.Stopwatch();
    public override void Awake()
    {
      base.Awake();
      Move(0, false, false, false);
      PreviousState = CurrentState;
      CurrentState = Behaviors.IDLE;
      floatY = this.m_Rigidbody2D.position.y;
      flyingEntity = m_IsFlying;
    }
    public override void Start()
    {
      initialPosition = new Point.Point(this.m_Rigidbody2D.position.x, this.m_Rigidbody2D.position.y);
      CurrentState = Behaviors.IDLE;
      string t = m_Rigidbody2D.transform.parent.parent.name;
      nextRandomDist = generateNextRandomIdleDistance();
    }
    public new void FixedUpdate()
    {
      base.FixedUpdate();

      EnemyBehavior();
    }

    protected virtual void EnemyBehavior()
    {
      switch (CurrentState)
      {
        case Behaviors.IDLE:
          Idle();
          if (flyingEntity)
            m_IsFlying = true;
          if ((this.facing == Direction.RIGHT && initialPosition.distanceTo(m_Rigidbody2D.position).x >= idleDistance + nextRandomDist)
            || !isGroundInFrontOf() || isWallInFrontOf()
            || IdleStopWatch.ElapsedMilliseconds >= 12000)
          {
            this.Flip();
            IdleStopWatch.Reset();
            IdleStopWatch.Start();
            nextRandomDist = generateNextRandomIdleDistance();
          }
          else if ((this.facing == Direction.LEFT && initialPosition.distanceTo(m_Rigidbody2D.position).x <= -idleDistance - nextRandomDist)
            || !isGroundInFrontOf() || isWallInFrontOf()
            || IdleStopWatch.ElapsedMilliseconds >= 12000)
          {
            this.Flip();
            IdleStopWatch.Reset();
            IdleStopWatch.Start();
            nextRandomDist = generateNextRandomIdleDistance();
          }
          else if (!IdleStopWatch.IsRunning)
            IdleStopWatch.Start();

          if (this.getDistanceToEntity(Platformer2DUserControl.m_Character).y < disengageDistance / 2 && this.getAbsoluteDistanceToEntity(Platformer2DUserControl.m_Character) < engageDistance)
            setNewState(Behaviors.MOVE_TO_PLAYER);

          break;
        case Behaviors.MOVE_TO_PLAYER:
          //LOGIC
          if (flyingEntity)
            m_IsFlying = false;
          Vector2 playerVector = this.getDistanceToEntity(Platformer2DUserControl.m_Character);
          MoveToPlayer();
          if (playerVector.magnitude > disengageDistance || playerVector.y > disengageDistance / 2)
          {
            setNewState(Behaviors.IDLE);
          }
          break;
        case Behaviors.ATTACK:
          //LOGIC
          Attack();
          break;
        default:
          setNewState(Behaviors.NULL);
          throw new Exception("WTF? Daniel T. Holtzclaw should fix this....");
          break;
      }
    }

    protected int generateNextRandomIdleDistance()
    {
      return randomIdleDist.Next(idleDistanceVariability*2) - idleDistanceVariability;
    }

    public virtual void MoveToPlayer()
    {
      this.Move(this.getHorizontalDirectionToEntity(Platformer2DUserControl.m_Character) * m_RunSpeed, false, false, false);
    }
    public override void Attack()
    {
      throw new NotImplementedException();
    }
    public virtual void Idle()
    {
      if (m_IsFlying)
        this.m_Rigidbody2D.position = new Vector2(this.m_Rigidbody2D.position.x, floatY);


      if (this.facing == Direction.RIGHT)
        this.Move(this.m_WalkSpeed, false, false, false, true);
      else if (this.facing == Direction.LEFT)
        this.Move(-this.m_WalkSpeed, false, false, false, true);
    }

    public Behaviors CurrentState = Behaviors.NULL;
    public Behaviors PreviousState = Behaviors.NULL;

    protected void setNewState(Behaviors newState)
    {
      PreviousState = CurrentState;
      CurrentState = newState;
    }

  }
}
