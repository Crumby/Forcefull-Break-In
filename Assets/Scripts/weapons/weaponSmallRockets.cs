using UnityEngine;
using System.Collections;

public class weaponSmallRockets : MonoBehaviour
{

    public GameObject projectile;
    public GameObject fireExplosion;
    public Transform[] spwawnsL;
    public Transform[] spwawnsR;

    public void Fire(Vector3 to)
    {
        if (Random.Range(0, 10) % 2 == 0)
        {
            foreach (var pos in spwawnsL)
            {
                Instantiate(fireExplosion, pos.position, Quaternion.identity);
                var tmp_1 = (GameObject)Instantiate(projectile, pos.position, Quaternion.identity);
                var tmp = tmp_1.GetComponent<motionProjectile>();
                tmp.transform.LookAt(to, Vector3.up);
                tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
                tmp.launch = true;
            }
        }
        else
        {
            foreach (var pos in spwawnsR)
            {
                Instantiate(fireExplosion, pos.position, Quaternion.identity);
                var tmp_1 = (GameObject)Instantiate(projectile, pos.position, Quaternion.identity);
                var tmp = tmp_1.GetComponent<motionProjectile>();
                tmp.transform.LookAt(to, Vector3.up);
                tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
                tmp.launch = true;
            }
        }
    }

}
