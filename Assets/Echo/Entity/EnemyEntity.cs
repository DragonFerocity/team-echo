using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Echo.Entity
{
  public class EnemyEntity : Entity
  {
    public new float m_WalkSpeed = 3f;
    public new float m_RunSpeed = 6f;
    private System.Diagnostics.Stopwatch IdleStopWatch = new System.Diagnostics.Stopwatch();
    public override void Awake()
    {
      base.Awake();
      Move(0, false, false);
      PreviousState = CurrentState;
      CurrentState = Behaviors.IDLE;
    }
    public override void Start()
    {
      CurrentState = Behaviors.IDLE;
    }
    public new void FixedUpdate()
    {
      base.FixedUpdate();

      switch (CurrentState)
      {
        case Behaviors.IDLE:
          PreviousState = CurrentState;
          CurrentState = Behaviors.IDLE;
          if (!IdleStopWatch.IsRunning)
            Idle();
          else if (IdleStopWatch.ElapsedMilliseconds >= 1000)
          {
            this.Flip();
            Idle();
          }
          if (this.getAbsoluteDistanceToEntity(Platformer2DUserControl.m_Character) < 2)
            CurrentState = Behaviors.MOVE_TO_PLAYER;

          break;
        case Behaviors.MOVE_TO_PLAYER:
          //LOGIC
          PreviousState = CurrentState;
          CurrentState = Behaviors.MOVE_TO_PLAYER;
          MoveToPlayer();
          break;
        case Behaviors.ATTACK:
          //LOGIC
          PreviousState = CurrentState;
          CurrentState = Behaviors.ATTACK;
          Attack();
          break;
        default:
          PreviousState = CurrentState;
          CurrentState = Behaviors.NULL;
          break;
      }
    }

    public virtual void MoveToPlayer()
    {
      this.Move(this.getHorizontalDirectionToEntity(Platformer2DUserControl.m_Character) * m_RunSpeed, false, false);
    }
    public override void Attack()
    {
      throw new NotImplementedException();
    }
    public virtual void Idle()
    {
      IdleStopWatch.Reset();
      IdleStopWatch.Start();
      if (this.facing == Direction.RIGHT)
        this.Move(this.m_WalkSpeed, false, false);
      else if (this.facing == Direction.LEFT)
        this.Move(-this.m_WalkSpeed, false, false);
    }

    public Behaviors CurrentState = Behaviors.NULL;
    public Behaviors PreviousState = Behaviors.NULL;

  }
}
