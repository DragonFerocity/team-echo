using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echo.Entity
{
  public abstract class Entity : MonoBehaviour
  {
    protected float m_WalkSpeed;
    protected int m_MaxHealth;

    protected abstract void Awake();
    protected abstract void Start();
    protected abstract void Update();
    protected abstract void FixedUpdate();
    protected abstract void Jump();
    protected abstract void attack();
  }
}