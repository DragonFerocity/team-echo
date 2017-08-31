using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
  protected float walkSpeed = 15f;
  protected static int health = 200; //This is just some number I put in

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

  protected void 
}
