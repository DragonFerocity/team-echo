using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
  protected abstract float walkSpeed {
    get;
    set;
  }
  protected abstract int maxHealth {
    get;
    set;
  }

  public abstract void Start();
  public abstract void Update();
  void jump()
  {
    //Jump Code here
  }
  protected void moveTo(float x, float y)
  {
    transform.Translate(x, y, 0);
  }

  protected abstract void attack();
}
