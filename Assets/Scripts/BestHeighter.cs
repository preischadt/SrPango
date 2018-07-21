using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestHeighter : MonoBehaviour {
	
	void Start()
	{
		transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sortingLayerName = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
		transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().sortingOrder = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingOrder;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(
			transform.position.x,
			PlayerPrefs.GetFloat("bestHeight", 0f)+1f,
			transform.position.z	
		);
	}
}
