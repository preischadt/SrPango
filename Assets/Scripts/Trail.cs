using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour {
	Transform target;

	/*
	void Start(){
		TrailRenderer tr = GetComponent<TrailRenderer>();
 		//tr.sortingLayerName = "PlayerTrail";
		//tr.sortingOrder = 50;
	}
	*/
	
	void Update () {
		if(target) transform.position = target.position;
	}

	public void SetTarget(Transform tg){
		target = tg;
	}

	public void Detach(){
		target = null;
	}
}
