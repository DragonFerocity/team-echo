using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Echo.Point
{
  public class Point
  {
    public Point(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public float x
    {
      get;
      set;
    }
    public float y
    {
      get;
      set;
    }
    public Vector2 vector
    {
      get
      {
        return new Vector2(x, y);
      }
    }
    public Vector2 distanceTo(Point pIn)
    {
      return new Vector2(pIn.x - this.x, pIn.y - this.y);
    }
    public Vector2 distanceTo(Vector2 pIn)
    {
      return new Vector2(pIn.x - this.x, pIn.y - this.y);
    }
    public Vector2 distanceTo(float xIn, float yIn)
    {
      return distanceTo(new Point(xIn, yIn));
    }
  }
}
