using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {
	Transform origin;
	Transform target;
	float speed = 35f;
	public Attack(Transform origin, Transform target){
		this.origin = origin;
		this.target = target;
	}

	public Vector2 GetSpeed(){
		if(target==null) return Vector2.zero;
		return speed*(target.position-origin.position).normalized;
	}
}

