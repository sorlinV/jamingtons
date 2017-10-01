using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private float dmg;
    private float delayed;
    private AudioSource audio;
    private AudioClip reload_sound;
    private AudioClip shoot_sound;
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
        this.dmg = dmg;
        this.nb_max_bullet = nb_bullet;
        this.bullet_per_shot = bullet_per_shot;
    }

    public void setAudios(AudioSource audio, AudioClip reload_sound, AudioClip shoot_sound)
    {
        this.audio = audio;
        this.reload_sound = reload_sound;
        this.shoot_sound = shoot_sound;
    }

    public string ui()
    {
        return this.name + ": " + this.nb_bullet + "/" + this.nb_max_bullet;
    }

    public void reload()
    {
        if (Time.time > delayed) {
            audio.PlayOneShot(reload_sound);
            delayed = Time.time + this.reloadDelay;
            nb_bullet = nb_max_bullet;
        }
    }

    float get_range(Vector3 a, Vector3 b){
        return (Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2) + Mathf.Pow(b.z - a.z, 2)));
    }

    public void shoot(Transform transform, Transform spawn_point, GameObject impact, GameObject bullet)
    {
        if (Time.time > delayed) {
            audio.PlayOneShot(shoot_sound);
            for (int i = 0; i < this.bullet_per_shot; i++)
            {
                RaycastHit hit;
                float rand = Random.Range(-this.spray, this.spray);
                Vector3 dir = Quaternion.Euler(0, rand, 0) *
                    spawn_point.forward;
                Ray ray = new Ray(spawn_point.position, dir);
                GameObject clone = GameObject.Instantiate(bullet, spawn_point.position,
                    Quaternion.Euler(spawn_point.eulerAngles.x, spawn_point.eulerAngles.y + rand, spawn_point.eulerAngles.z)) as GameObject;
                GameObject.Destroy(clone, 2f);
                if (Physics.Raycast(ray, out hit, this.range))
                {
                    if (get_range(hit.point, spawn_point.position) < this.range) {
                        //went something is touch by bullet
                        if (hit.collider.gameObject.GetComponent<enemy>())
                        {
                            hit.collider.gameObject.GetComponent<enemy>().hpAction(-this.dmg);
                        }
                        clone = GameObject.Instantiate(impact, hit.point, transform.rotation) as GameObject;
                        GameObject.Destroy(clone, 1f);
                    }
                }
            }
            nb_bullet--;
            if (nb_bullet <= 0)
            {
                this.reload();
            } else {
                delayed = Time.time + this.delay;
            }
        }
    }

}

[RequireComponent(typeof(AudioSource))]
public class player : MonoBehaviour {
    public Transform bullet_spawn;
	public GameObject impact;
	public GameObject bullet;
    public Text weapon_ui;
    public Text hp_ui;
	public float speed = 10;
	public bool is_falling = true;
    public float hp = 100;
    private float hpMax;
    private Weapon current_weapon;
    public void hpAction (float dmg) {
        hp += dmg;
        if (hp <= 0)
        {
            died();
            hp = 0;
        } else if(hp > hpMax) {
            hp = hpMax;
        }
    }


    public AudioClip reload_ak;
    public AudioClip shoot_ak;
    public AudioClip reload_pistol;
    public AudioClip shoot_pistol;
    public AudioClip reload_shootgun;
    public AudioClip shoot_shootgun;
	private Weapon shootgun = new Weapon("shootgun", 0.7f, 3.8f, 10f, 20f, 20f, 6, 10);
	private Weapon pistol = new Weapon("pistol", 0.4f, 1.60f, 2.5f, 35f, 35f, 12, 1);
	private Weapon ak = new Weapon("ak", 0.1f, 1.60f, 5f, 45f, 15f, 30, 1);
    void Start()
    {
        hpMax = hp;
        ak.setAudios(GetComponent<AudioSource>(), reload_ak, shoot_ak);
        pistol.setAudios(GetComponent<AudioSource>(), reload_pistol, shoot_pistol);
        shootgun.setAudios(GetComponent<AudioSource>(), reload_shootgun, shoot_shootgun);
        current_weapon = pistol;
    }


	IEnumerator setTimeout(Funct callback, float time) {
        yield return new WaitForSeconds(time);
		callback();
	}

    public void died()
    {
		SceneManager.LoadScene("menu");
    }


	void mouv()
	{
		Vector3 dir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
		if (is_falling == true)
		{
			dir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal") - transform.up;
		}
        if (dir.x != 0 || dir.z != 0)
        {
            GetComponentInChildren<Animation>().Play("Armature_player|Player_walk");
        }
        else
        {
            GetComponentInChildren<Animation>().Play("Armature_player|Player_idle");
        }
        if (dir.x != 0 || dir.y != 0 || dir.z != 0) {
            transform.gameObject.GetComponent<Rigidbody>().velocity = dir.normalized * speed;
		}
		transform.Rotate(new Vector3(0, Input.GetAxis("Mouse ScrollWheel") * speed * 10, 0));
	}

    void ui ()
    {
        hp_ui.text = "";
        int hp_view = (int)(hp/hpMax * 10);
        for (int i = 0; i < hp_view; i++) {
            hp_ui.text += "♥";
        }
        weapon_ui.text = current_weapon.ui();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) {
            current_weapon = pistol;
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            current_weapon = shootgun;
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            current_weapon = ak;
        }
        mouv();
        if (Input.GetButton("Fire1"))
        {
            current_weapon.shoot(transform, bullet_spawn, impact, bullet);
        }
        if (Input.GetKey(KeyCode.R))
        {
            current_weapon.reload();
        }
        ui();
	}
 
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "member") {
			hpAction(10);
			Destroy(other.gameObject);
		}
	}
}
