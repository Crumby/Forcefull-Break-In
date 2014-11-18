using UnityEngine;
using System.Collections;

public class weaponBlastCanon : MonoBehaviour {

    public GameObject projectile;

    public void Fire(Vector3 to)
    {
        var tmp_1 = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
        var tmp = tmp_1.GetComponent<motionProjectile>();
        tmp.transform.LookAt(to, Vector3.up);
        tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
        tmp.launch = true;
    }
}
