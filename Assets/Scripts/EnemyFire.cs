using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour
{

    public GameObject projectile;

    // Use this for initialization
    void Start()
    {
        projectile.GetComponent<ProjectileMotion>().OwnerStats = this.GetComponent<Stats>();
    }

    void Fire()
    {
        var hlp = (GameObject)Instantiate(projectile, new Vector3(transform.position.x,
                    transform.position.y,
                   transform.position.z - collider.bounds.size.z), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 2526) % 256 == 0)
            Fire();
    }
}
