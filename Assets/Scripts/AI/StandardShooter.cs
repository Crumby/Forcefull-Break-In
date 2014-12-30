using UnityEngine;
using System.Collections;

public class StandardShooter : MonoBehaviour
{

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire = 0.0f;

    private GameObject target;
    public float minRange;
    public float maxRange;
    private float distance;

    void Update()
    {
        if (!gameData.pausedGame && GetComponentInParent<basicEnemySystems>() != null && GetComponentInParent<basicEnemySystems>().health > 0)
        {
            target = GameObject.FindGameObjectWithTag("BullsEye");
            if (target != null)
            {
                distance = Vector3.Distance(transform.position, target.transform.position);
                if ((distance > minRange) && (distance < maxRange))
                {
                    var hits = Physics.RaycastAll(transform.position, gameData.playerPosition - transform.position);
                    if (hits.Length == 0)
                        if (Time.time > nextFire && this.renderer.isVisible)
                        {
                            nextFire = Time.time + fireRate;
                            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                            //audio.Play ();
                        }
                }
            }
        }
    }
}
