using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	protected Rigidbody2D rb;
	float spawnDistance = 100f;
	protected GameObject player;
	float variation = 0.2f;
	float maxDistance = 30f;

	void Awake () {
		rb = GetComponent<Rigidbody2D>();
	}
	protected void Update()
	{
		//if distance to player is too big, self-destroy
		if(player!=null && (player.transform.position-transform.position).magnitude>maxDistance){
			Destroy(gameObject);
		}
	}

	public void Initialize(Vector2 pos, GameObject player, float speed){
		this.player = player;
		float angle = Random.Range(0f, 2*Mathf.PI);
		Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		rb.velocity = direction*speed;
		
		rb.position = -direction*spawnDistance + new Vector2(
			variation*16f*Random.Range(-0.5f, 0.5f),
			variation*9f*Random.Range(-0.5f, 0.5f)
		);

		//spawn on exact screen border
		float bestT = float.MaxValue;
		float bestT2 = float.MaxValue;
		float t;
		
		//x==-10:
		t = (-10f-rb.position.x)/rb.velocity.x;
		if(t<bestT){
			bestT2 = bestT;
			bestT = t;
		}else if(t<bestT2){
			bestT2 = t;
		}
		
		//x==10:
		t = (10f-rb.position.x)/rb.velocity.x;
		if(t<bestT){
			bestT2 = bestT;
			bestT = t;
		}else if(t<bestT2){
			bestT2 = t;
		}
		
		//y==-6:
		t = (-6f-rb.position.y)/rb.velocity.y;
		if(t<bestT){
			bestT2 = bestT;
			bestT = t;
		}else if(t<bestT2){
			bestT2 = t;
		}

		//y==6:
		t = (6f-rb.position.y)/rb.velocity.y;
		if(t<bestT){
			bestT2 = bestT;
			bestT = t;
		}else if(t<bestT2){
			bestT2 = t;
		}
		
		//use best t to choose new position
		rb.position = new Vector2(
			pos.x + rb.position.x + rb.velocity.x*bestT2,
			pos.y + rb.position.y + rb.velocity.y*bestT2
		);
	}
}
