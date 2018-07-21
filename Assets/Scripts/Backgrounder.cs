using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounder : MonoBehaviour {

	float tiltY = 10f;
	public GameManager manager;
	GameObject[] rep;

	void Awake()
	{
		rep = new GameObject[transform.childCount];
		for(int i=0; i<transform.childCount; i++){
			rep[i] = transform.GetChild(i).gameObject;
		}
	}

	void LateUpdate(){
		int y = manager.GetY(transform.position.y+tiltY);
		if(y<manager.GetLevel2y()){
			Activate(0);
		}else if(y==manager.GetLevel2y()){
			Activate(1);
		}else if(y<manager.GetLevel3y()){
			Activate(2);
		}else if(y==manager.GetLevel3y()){
			Activate(3);
		}else{
			Activate(4);
		}
	}

	void Activate(int id){
		int i=0;
		foreach(GameObject g in rep){
			g.SetActive(i==id);
			++i;
		}
	}
}
