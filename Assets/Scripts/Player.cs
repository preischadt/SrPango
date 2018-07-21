	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	Rigidbody2D rb;
	float maxFallSpeed = 10f;
	float horSpeed = 8f;
	float bounceSpeed = 20f;
	float attackRotationSpeed = 10000f;
	Attack attacking = null;
	float deaccel = 0.1f;
	float maxAttackDistance = 9f;
	public GameObject trailPrefab;
	public Text score;
	Trail trail;
	GameObject normalChild;
	GameObject attackChild;
	Animator anim;
	float stunTime = 0.2f;
	float stunned;
	AudioSource[] sound;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		normalChild = transform.GetChild(0).gameObject;
		attackChild = transform.GetChild(1).gameObject;
		anim = GetComponent<Animator>();
		sound = GetComponents<AudioSource>();
		//PlayerPrefs.SetFloat("bestHeight", 10); //TODO deleta
	}

	// Update is called once per frame
	void Update () {
		if(Time.timeScale==0f) return;

		Vector2 speed = rb.velocity;

		//finish stun
		if(stunned>0){
			stunned -= Time.deltaTime;
			rb.rotation = 180f;
		}else{
			if(attacking==null){
				//move
				speed.x *= Mathf.Pow(deaccel, Time.deltaTime);
				if(IsHoldingLeft()){
					speed.x = Mathf.Min(-horSpeed, speed.x);
				}else if(IsHoldingRight()){
					speed.x = Mathf.Max(horSpeed, speed.x);
				}

				//attack
				if(IsPressingAttack()){
					Attack();
				}

				//limit fall speed
				speed.y = Mathf.Max(speed.y, -maxFallSpeed);

				//switch side and tilt
				if(speed.x>0){
					transform.localScale = new Vector2(1, 1);
					rb.rotation = speed.y;
				}else{
					transform.localScale = new Vector2(-1, 1);
					rb.rotation = -speed.y;
				}
			}else{
				//continue attack
				speed = attacking.GetSpeed();
				if(speed==Vector2.zero){
					//lost target, cancel attack
					FinishAttack();
				}else{
					//continue Attack
					rb.angularVelocity = speed.x>0? -attackRotationSpeed : attackRotationSpeed;
				}
			}
		}

		//update speed
		rb.velocity = speed;

		//update score
		float height = rb.position.y;		
		score.text = " " + (Mathf.RoundToInt(rb.position.y)-1) + "m";
		if(height>PlayerPrefs.GetFloat("bestHeight", 0f)){
			PlayerPrefs.SetFloat("bestHeight", height);
		}

		/*
		//TODO DELETA ISSO
		if(Input.GetKey(KeyCode.UpArrow)){
			Bounce();
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			Stun();
		}
		*/
	}

	void Bounce(){
		if(attacking!=null){
			sound[1].Stop();
			sound[1].Play();
		}
		FinishAttack();
		rb.velocity = new Vector2(rb.velocity.x, bounceSpeed);
	}

	void Stun(){
		FinishAttack();
		rb.velocity = Vector2.down*maxFallSpeed;
		if(stunned<=0){
			sound[2].Stop();
			sound[2].Play();
		}
		stunned = stunTime;
	}

	void Attack(){
		//get closest enemy below
		Transform closest = null;
		float minDist = float.MaxValue;
		int minSide = -1;
		int bestSide = 2; //must be different from initial minSide!
		if(IsHoldingLeft()){
			bestSide = 0;
		}else if(IsHoldingRight()){
			bestSide = 1;
		}
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Bounceable")){
			//if(go.transform.position.y>=transform.position.y) continue; //ignore enemies that aren't below 

			float dist = (go.transform.position - transform.position).magnitude;
			int side = go.transform.position.x<transform.position.x? 0:1;
			//float dy = Mathf.Abs(go.transform.position.y - transform.position.y);
			//if(dy>maxAttackDistance) continue; //ignore enemies that are too far
			Renderer r = go.GetComponent<Renderer>();
			if(r==null) r = go.transform.parent.GetComponent<Renderer>();
			if(!r.isVisible) continue; //ignore enemies outside camera

			//this side is better than previous best, or they are equally good but this is closer
			if((side==bestSide && minSide!=bestSide) || ((side==bestSide || minSide!=bestSide) && dist<minDist)){
				closest = go.transform;
				minDist = dist;
				minSide = side;
			}
		}
		if(closest){
			//play sound
			sound[0].Stop();
			sound[0].Play();

			trail = Instantiate(trailPrefab, transform.position, Quaternion.identity).GetComponent<Trail>();
			trail.SetTarget(transform);
			attacking = new Attack(transform, closest);
			normalChild.SetActive(false);
			attackChild.SetActive(true);
			anim.SetBool("attacking", true);
		}
	}

	void FinishAttack(){
		if(attacking!=null){
			trail.Detach();
			attacking = null;
		}
		rb.angularVelocity = 0f;
		normalChild.SetActive(true);
		attackChild.SetActive(false);
		anim.SetBool("attacking", false);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		//if stunned, ignore colliders
		if(stunned>0) return;

		switch(col.tag){
			case "Bounceable":
				//always bounce (kid friendly)
				Bounce();
				break;
			case "Obstacle":
				//get stunned if hit by obstacle
				Stun();
				break;
			case "Ground":
				//don't stop attack if ground
				break;
			case "UI":
				//ignore UI
				break;
			default:
				//stop attack if collides with anything else
				FinishAttack();
				break;
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		OnTriggerEnter2D(col);
	}

	void OnCollisionEnter2D(Collision2D col){
		OnTriggerEnter2D(col.collider);
	}

	bool IsHoldingLeft(){
		//if stunned, ignore controls
		return stunned<=0 && Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4);
	}
	
	bool IsHoldingRight(){
		//if stunned, ignore controls
		return stunned<=0 && Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6);
	}

	bool IsPressingAttack(){
		//if stunned, ignore controls
		return stunned<=0 && Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
	}
}
