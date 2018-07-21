using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	float maxCamDiff = 3f;
	float minY;
	AudioSource sound;
	bool canPlay;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
		minY = transform.position.y;
		canPlay = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pos = target.position;
		float y = Mathf.Max(minY, Mathf.Max(pos.y-maxCamDiff, Mathf.Min(pos.y+maxCamDiff, transform.position.y)));

		//scream if camera is going down
		if(y<transform.position.y){
			if(canPlay){
				sound.Play();
				canPlay = false;
			}
		}else{
			sound.Stop();
			canPlay = true;
		}
		//stop screaming if camera is going up

		transform.position = new Vector3(
			transform.position.x,
			y,
			transform.position.z
		);
	}
}
