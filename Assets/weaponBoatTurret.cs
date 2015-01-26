using UnityEngine;
using System.Collections;

public class weaponBoatTurret : MonoBehaviour
{

    public GameObject projectile;
    public Transform where;

    public void Fire(Vector3 to)
    {
        Debug.Log("FIRE");
        var tmp_1 = (GameObject)Instantiate(projectile, where.position, Quaternion.identity);
        var tmp = tmp_1.GetComponent<motionProjectile>();
        tmp.transform.LookAt(to, Vector3.up);
        tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
        tmp.launch = true;
    }
}
