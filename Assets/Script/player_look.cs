using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_look : MonoBehaviour {
	void look()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			transform.GetChild(0).LookAt(new Vector3(hit.point.x, transform.GetChild(0).position.y, hit.point.z));
		}
	}

	void Update()
	{
		look();
	}
}
