using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Funct();

public class player : MonoBehaviour {
	public GameObject bullet;
	public float speed = 10;
	float delayed_shoot = 0;

	IEnumerator setTimeout(Funct callback, float time) {
        yield return new WaitForSeconds(time);
		callback();
	}

	void shoot (float delay, int nb_bullet, float spray) {
		if (Time.time > delayed_shoot) {
			delayed_shoot = Time.time + delay;
			for (int i = 0; i < nb_bullet; i++)
			{
				Quaternion dir = new Quaternion(transform.GetChild(0).rotation.x,
					transform.GetChild(0).rotation.y + Random.Range(-spray, spray),
					transform.GetChild(0).rotation.z, 0);
				Instantiate(bullet, transform.GetChild(0).position, dir);
			}
		}
	}

	void mouv()
	{
		Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		transform.gameObject.GetComponent<Rigidbody>().velocity = (dir) * speed * Time.deltaTime;
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * speed, 0));
	}

    void Update()
    {
		mouv();
		if (Input.GetButton("Fire1"))
		{
			shoot(0.5f, 3, 25f);
		}
	}
}
