using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageenemy : MonoBehaviour {
//used to deal damage to the enemies
	// Use this for initialization
	public int p_damage_amount = 6;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
			
	}
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Enemy")
		{
			Destroy(other.gameObject);
			//other.gameObject.GetComponent<Healthsystem>().take_damage(p_damage_amount);

		}
	}
}
