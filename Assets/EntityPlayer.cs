using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPlayer : Entity {

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
}
