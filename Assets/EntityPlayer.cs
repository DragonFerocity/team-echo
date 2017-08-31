using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : Entity {
  protected override int maxHealth {
    get
    {
      return 200;
    }
    set
    {
      this.maxHealth = value;
    }
  }
  protected override float walkSpeed
  {
    get
    {
      return 15f;
    }
    set
    {
      this.walkSpeed = value;
    }
  }

	// Use this for initialization
	public override void Start () {
		
	}
	
	// Update is called once per frame
	public override void Update () {
    move();
	}

  private void move()
  {
    var x = Input.GetAxis("Horizontal") * Time.deltaTime * walkSpeed;
    var y = Input.GetAxis("Vertical") * Time.deltaTime * walkSpeed;

    moveTo(x, y);
  }

  protected override void attack()
  {
    //Attack code here!
  }
}
