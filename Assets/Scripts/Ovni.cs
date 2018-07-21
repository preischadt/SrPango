using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ovni : Enemy {
	new public void Initialize(Vector2 pos, GameObject player, float diffMul){
		base.Initialize(pos, player, 8f*diffMul);
	}
}
