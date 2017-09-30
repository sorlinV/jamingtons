using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Funct();

public class player : MonoBehaviour {
	public GameObject bullet;
	public float speed = 10;
	public bool is_falling = true;
	float delayed_shoot = 0;
	
	IEnumerator setTimeout(Funct callback, float time) {
        yield return new WaitForSeconds(time);
		callback();
	}

	void shoot (float delay, float spray, float range, int nb_bullet) {
		if (Time.time > delayed_shoot) {
			delayed_shoot = Time.time + delay;
			for (int i = 0; i < nb_bullet; i++)
			{
				RaycastHit hit;
				Vector3 dir = Quaternion.Euler(0, Random.Range(-spray, spray), 0) * transform.GetChild(0).forward ;
		        Ray ray = new Ray(transform.position, dir);
				if (Physics.Raycast(ray, out hit, range))
				{
					Instantiate(bullet, hit.point, transform.rotation);
				}
			}
		}
	}

	void mouv()
	{
		Vector3 dir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
		if (is_falling == true)
		{
			dir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") - transform.up;
		}
		if (dir.x != 0 || dir.y != 0 || dir.z != 0) {
			transform.gameObject.GetComponent<Rigidbody>().velocity = dir * speed;
		}
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * speed * 10, 0));
	}

    void Update()
    {
		mouv();
		if (Input.GetButton("Fire1"))
		{
			shoot(0.5f, 15f, 30f, 5);
		}
	}
}
