using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Echo.Entity
{
  public interface IEntity
  {
    void Awake();
    void Start();
    void FixedUpdate();
  }
}
