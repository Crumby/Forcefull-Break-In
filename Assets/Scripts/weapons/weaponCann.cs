using UnityEngine;
using System.Collections;

public class weaponCann : MonoBehaviour {

    public GameObject projectile;
    public GameObject fireExplision;
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
            bullets[p_rail] = (GameObject)Instantiate(projectile, spawns[p_rail].position, Quaternion.Euler(new Vector3(0,180,0)));
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
        if (timer < fireRate)
            timer += Time.deltaTime;
    }

    public void Fire()
    {
        if (timer >= fireRate)
        {
            var tmp_1 = bullets[Rail];
            if (tmp_1 != null)
            {
                Instantiate(fireExplision,tmp_1.transform.position,Quaternion.identity);
                bullets[p_rail].transform.parent = null;
                tmp_1.SetActive(true);
                var tmp = bullets[p_rail].GetComponent<motionProjectile>();
                tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
                tmp.isPlayers = false;
                tmp.launch = true;
            }
        }
    }
}
