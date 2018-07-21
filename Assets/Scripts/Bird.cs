using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag=="Player"){
			//play sound
			AudioSource aud = GetComponent<AudioSource>();
			aud.Stop();
			aud.Play();
		}
	}
}
