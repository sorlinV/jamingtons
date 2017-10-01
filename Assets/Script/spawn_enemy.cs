using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_enemy : MonoBehaviour {
	private float delayed_time;
	public float delay = 20;
	public GameObject[] entity;

	void Start()
	{
		transform.GetChild(0).gameObject.SetActive(false);
	}

	void Update () {
		if (Time.time > delayed_time)
		{
			delay = delay * 0.95f;
			delayed_time = Time.time + delay;
			Vector3 rand = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
			Instantiate(entity[(int) Random.Range(-0.5f, entity.Length-0.5f)], transform.position + rand, transform.rotation);
		}
	}
}
