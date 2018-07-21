using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollow : MonoBehaviour {

	new public Transform camera;
	float height;

	void Awake () {
		height = transform.GetChild(0).position.y - transform.GetChild(1).position.y;
	}
	
	void Update () {
		Vector3 pos = transform.position;
		float diff = camera.position.y - transform.position.y;
		if(diff>height){
			pos.y += height;
			transform.position = pos;
		}
		if(diff<-height){
			pos.y -= height;
			transform.position = pos;
		}
	}
}
