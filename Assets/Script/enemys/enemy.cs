using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour {
	public GameObject[] corpParts;
	public bool is_falling = true;
	public float delay_att = 2f;
	private float delayed_att;
    public float hp = 100;
    private float hpMax;

	void Start ()
	{
		hpMax = hp;
		foreach (GameObject corpPart in corpParts)
		{
			corpPart.SetActive(Random.value < 0.5);
		}
	}

	public void died ()
	{
		Destroy(gameObject);
	}

    public void hpAction (float dmg) {
        hp += dmg;
        if (hp <= 0)
        {
            died();
            hp = 0;
        } else if(hp > hpMax) {
            hp = hpMax;
        }

		int i = 0;
		foreach (GameObject corpPart in corpParts)
		{
			if (corpPart.activeSelf) {
				i++;
			}
		}
		if (Random.Range(0, 20) < 1) {
			int rand = (int)Random.Range(0, i);
			i = -1;
			foreach (GameObject corpPart in corpParts)
			{
				if (corpPart.activeSelf) {
					i++;
					if (i == rand)
					{
						Instantiate(Resources.Load(corpPart.name), transform.position, transform.rotation);
						corpPart.SetActive(false);
					}
				}
			}
		}
    }

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
		if (close_range < 2 && delayed_att < Time.time)
		{
			delayed_att = Time.time + delay_att;
			close.GetComponent<player>().hpAction(-10);
            GetComponentInChildren<Animation>().Play("Armature_zombi|zombi_attack");
        } else if (close_range >= 2) {
			GetComponent<NavMeshAgent>().destination = close.transform.position;
            GetComponentInChildren<Animation>().Play("Armature_zombi|zombi_walk");
        }
	}
}
