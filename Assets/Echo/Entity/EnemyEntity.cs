using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Echo.Entity
{
  class EnemyEntity : Entity
  {
    public override void Awake()
    {
      base.Awake();
      Move(0, false, false);
    }
    public override void Start()
    {

    }
    public new void FixedUpdate()
    {
      base.FixedUpdate();
    }
    public override void attack()
    {
      throw new NotImplementedException();
    }
  }
}
