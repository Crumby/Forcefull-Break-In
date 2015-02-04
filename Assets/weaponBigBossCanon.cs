using UnityEngine;
using System.Collections;

public class weaponBigBossCanon : MonoBehaviour
{

    public GameObject projectile;
    public GameObject explosion;
    public Transform[] whereL, whereR;
    private float miss = 500;

    public void Fire(Vector3 to)
    {
        for (int i = 0; i < whereL.Length; i++)
        {
            Instantiate(explosion, whereL[i].position, Quaternion.identity);
            var tmp_1 = (GameObject)Instantiate(projectile, whereL[i].position, Quaternion.identity);
            var tmp = tmp_1.GetComponent<motionProjectile>();
            tmp.transform.LookAt(to + new Vector3(i * miss, 0, 0), Vector3.up);
            tmp.launch = true;
        }
        for (int i = 0; i < whereR.Length; i++)
        {
            Instantiate(explosion, whereR[i].position, Quaternion.identity);
            var tmp_1 = (GameObject)Instantiate(projectile, whereR[i].position, Quaternion.identity);
            var tmp = tmp_1.GetComponent<motionProjectile>();
            tmp.transform.LookAt(to + new Vector3(-i * miss, 0, 0), Vector3.up);
            tmp.launch = true;
        }
    }
}
