using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Echo.Entity
{
  public enum Behaviors : Byte
  {
    NULL = 0,
    IDLE,
    MOVE_TO_PLAYER,
    ATTACK,
    FLY_TO_PLAYER
  }
}
