using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class history_scroll : MonoBehaviour {

	public void speed ()
	{
		transform.Translate(0,15f,0);		
	}
	
	void Update () {
		transform.Translate(0,0.1f,0);
	}
}
