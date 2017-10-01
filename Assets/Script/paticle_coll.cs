using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paticle_coll : MonoBehaviour {
	void OnParticleCollision (GameObject obj) {
		print(obj.name);
	}
}
