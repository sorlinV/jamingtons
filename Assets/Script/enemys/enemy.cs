using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour {
	public bool is_falling = true;

    float get_range(Vector3 a, Vector3 b){
        return (Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.z - a.z, 2)));
    }

	void Update () {
		float close_range = -1f;
		GameObject close = GameObject.FindGameObjectWithTag("Player");
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (get_range(player.transform.position, transform.position) < close_range || close_range == -1f)
			{
				close = player;
				close_range = get_range(player.transform.position, transform.position);
			}
		}
		if (close_range < 2)
		{

		} else {
			GetComponent<NavMeshAgent>().destination = close.transform.position;
		}
	}
}
