using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_enemy : MonoBehaviour {
	private float delayed_time;
	public float delay = 2;
	public GameObject entity;

	void Start()
	{
		transform.GetChild(0).gameObject.SetActive(false);
	}

	void Update () {
		if (Time.time > delayed_time)
		{
			delayed_time = Time.time + delay;
			Instantiate(entity, transform.position, transform.rotation);
		}
	}
}
