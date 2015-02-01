using UnityEngine;
using System.Collections;

public class weaponRailGun : MonoBehaviour
{

    public GameObject projectile;
    public Transform[] spawns;
    [HideInInspector]
    public GameObject[] bullets;
    [Range(0, 0.5F)]
    public float fireRate;
    private float timer;
    private int p_rail = 0;
    public int Rail
    {
        get
        {
            bullets[p_rail] = (GameObject)Instantiate(projectile, spawns[p_rail].position, Quaternion.identity);
            bullets[p_rail].SetActive(false);
            bullets[p_rail].transform.parent = transform;
            p_rail = (p_rail + 1) % spawns.Length;
            timer = 0;
            return p_rail;
        }
    }

    void Start()
    {
        bullets = new GameObject[spawns.Length];
    }

    void Update()
    {
        if (timer < fireRate/gameData.firespeedCannon)
            timer += Time.deltaTime;
    }

    public void Fire()
    {
        if (timer >= fireRate)
        {
            var tmp_1 = bullets[Rail];
            if (tmp_1 != null)
            {
                bullets[p_rail].transform.parent = null; 
                tmp_1.SetActive(true);
                tmp_1.GetComponent<AudioSource>().Play();
                var tmp = bullets[p_rail].GetComponent<motionProjectile>();
                tmp.forwardSpeed += GetComponentInParent<motionPlayer>().forwardSpeed;
                if (gameData.aimPoint != Vector3.zero)
                    tmp.transform.LookAt(gameData.aimPoint);
                else
                    tmp.transform.rotation = transform.rotation;
                tmp.isPlayers = true;
                tmp.destroyDmg += gameData.bonusDmgCannon;
                tmp.launch = true;
            }
        }
    }
}
