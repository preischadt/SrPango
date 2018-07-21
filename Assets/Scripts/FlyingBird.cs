using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBird : Enemy {
	
	bool stunned;
	float cooldown;
	float dieTime = 5f;

	new public void Initialize(Vector2 pos, GameObject player, float diffMul){
		base.Initialize(pos, player, 6f*diffMul);
	}
	new void Update()
	{
		base.Update();

		//after stunned, die
		if(stunned){
			cooldown += Time.deltaTime;
			if(cooldown>=dieTime){
				rb.bodyType = RigidbodyType2D.Dynamic;
				gameObject.tag = "Untagged";
			}
		}

		//set sprite direction
		if(rb.velocity.x<0){
			transform.localScale = new Vector3(-1, 1, 1);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Player"){
			//play sound
			AudioSource aud = GetComponent<AudioSource>();
			aud.Stop();
			aud.Play();

			//get stunned if attacked;
			rb.velocity = Vector2.zero;
			GetComponent<Animator>().speed = 0f;
			stunned = true;
		}
	}

	
}
