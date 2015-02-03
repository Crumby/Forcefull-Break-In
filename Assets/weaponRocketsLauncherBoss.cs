using UnityEngine;
using System.Collections;

public class weaponRocketsLauncherBoss : MonoBehaviour
{
    public GameObject projectile;
    public Transform[] whereUp, whereDown;
    private float miss = 75;

    public void Fire(Vector3 to)
    {
        for (int i = 0; i < whereUp.Length; i += 1 + Random.Range(0, 25) % 2)
        {
            var tmp_1 = (GameObject)Instantiate(projectile, whereUp[i].position, Quaternion.identity);
            var tmp = tmp_1.GetComponent<motionProjectile>();
            float hlp = 1;
            if (miss * Random.Range(1, 100) % 2 == 0)
                hlp = -1;
            tmp.transform.LookAt(to + new Vector3(hlp * miss * Random.Range(1, 100) / 100f, miss * Random.Range(1, 100) / 100f, 0), Vector3.up);
            tmp.launch = true;
        }
        for (int i = 0; i < whereDown.Length; i += 1 + Random.Range(0, 25) % 2)
        {
            var tmp_1 = (GameObject)Instantiate(projectile, whereDown[i].position, Quaternion.identity);
            var tmp = tmp_1.GetComponent<motionProjectile>();
            float hlp = 1;
            if (miss * Random.Range(1, 100) % 2 == 0)
                hlp = -1;
            tmp.transform.LookAt(to + new Vector3(hlp * miss * Random.Range(1, 100) / 100f, -miss * Random.Range(1, 100) / 100f, 0), Vector3.up);
            tmp.launch = true;
        }
    }
}
