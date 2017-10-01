using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public delegate void Funct();

class Weapon
{
    private string name;
    private float delay;
    private float reloadDelay;
    private float spray;
    private float range;
    private int nb_bullet;
    private int nb_max_bullet;
    private int bullet_per_shot;
    private int dmg;
    private float delayed = 0;
    public Weapon(string name,
        float delay,
        float reloadDelay,
        float spray,
        float range,
        float dmg,
        int nb_bullet,
        int bullet_per_shot)
    {
        this.name = name;
        this.delay = delay;
        this.reloadDelay = reloadDelay;
        this.spray = spray;
        this.range = range;
        this.nb_bullet = nb_bullet;
        this.nb_max_bullet = nb_bullet;
        this.bullet_per_shot = bullet_per_shot;
    }

    public void reload(AudioSource audio, AudioClip reload)
    {
        if (Time.time > delayed) {
            audio.PlayOneShot(reload);
            delayed = Time.time + this.reloadDelay;
            nb_bullet = nb_max_bullet;
        }
    }

    float get_range(Vector3 a, Vector3 b){
        return (Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.z - a.z, 2)));
    }

    public void shoot(Transform transform, Transform spawn_point, GameObject impact, GameObject bullet,
    AudioSource audio, AudioClip shoot, AudioClip reload)
    {
        if (Time.time > delayed) {
            audio.PlayOneShot(shoot);
            for (int i = 0; i < this.bullet_per_shot; i++)
            {
                RaycastHit hit;
                Vector3 dir = Quaternion.Euler(0, Random.Range(-this.spray, this.spray), 0) *
                    spawn_point.forward;
                Ray ray = new Ray(transform.position, dir);
                GameObject clone = GameObject.Instantiate(bullet, spawn_point.position,
                    spawn_point.rotation) as GameObject;
                GameObject.Destroy(clone, 2f);
                if (Physics.Raycast(ray, out hit, this.range))
                {
                    if (get_range(hit.point, transform.position) < this.range) {
                        //went something is touch by bullet
                        clone = GameObject.Instantiate(impact, hit.point, transform.rotation) as GameObject;
                        GameObject.Destroy(clone, 1f);
                    }
                }
            }
            nb_bullet--;
            if (nb_bullet <= 0)
            {
                this.reload(audio, reload);
            } else {
                delayed = Time.time + this.delay;
            }
        }
    }

}

public class player : NetworkBehaviour {
    public Transform bullet_spawn;
	public GameObject impact;
	public GameObject bullet;
	public float speed = 10;
	public bool is_falling = true;
    public float hp = 100;
    public AudioClip pistol_shoot;
    public AudioClip pistol_reload;
    public AudioClip shootgun_shoot;
    public AudioClip shootgun_reload;
    public AudioClip ak_shoot;
    public AudioClip ak_reload;
    private float hpMax;
    void Start()
    {
        hpMax = hp;
    }

	// Weapon shootgun = new Weapon("shootgun", 0.7f, 3f, 10f, 20f, 20f, 7, 10);
	// Weapon pistol = new Weapon("pistol", 0.4f, 1.60f, 2.5f, 35f, 35f, 12, 1);
	Weapon ak = new Weapon("ak", 0.1f, 1.60f, 5f, 45f, 15f, 30, 1);

	IEnumerator setTimeout(Funct callback, float time) {
        yield return new WaitForSeconds(time);
		callback();
	}

    public void lost_hp (float dmg) {

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
        if (!isLocalPlayer) {
            return;
        }
        mouv();
        if (Input.GetButton("Fire1"))
        {
            ak.shoot(transform, bullet_spawn, impact, bullet, GetComponent<AudioSource>(), ak_shoot, ak_reload);
        }
        if (Input.GetKey(KeyCode.R))
        {
            ak.reload(GetComponent<AudioSource>(), ak_reload);
        }

	}
}
