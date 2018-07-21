using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTextAutoOrder : MonoBehaviour {
	void Awake()
	{
		MeshRenderer mr = GetComponent<MeshRenderer>();
		mr.sortingLayerName = "UI";
		mr.sortingOrder = -1;
	}
}
