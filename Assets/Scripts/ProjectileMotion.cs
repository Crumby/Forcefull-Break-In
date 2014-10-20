using UnityEngine;
using System.Collections;

public class ProjectileMotion : MonoBehaviour
{
    private float timeToLive = 5;
    public int speed = 500;
    public int Dmg = 50;
    public Stats OwnerStats;
    public GameObject explosion, bigExplosion;
    public bool Forward = true;

    void Update()
    {
        if (!PlayerMotion.Pause)
        {
            timeToLive -= Time.deltaTime;
            if (Forward)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            else
                transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (timeToLive <= 0)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.GetComponent<Stats>() == OwnerStats ||
            (collision.gameObject.GetComponent<ProjectileMotion>() != null && collision.gameObject.GetComponent<ProjectileMotion>().OwnerStats == OwnerStats)))
        {
            Destroy(Instantiate(explosion, collision.contacts[0].point, Quaternion.identity), 2);
            var stats = collision.gameObject.GetComponent<Stats>();
            if (stats != null)
            {
                if (stats.Shield - Dmg <= 0)
                {
                    stats.Hp = stats.Hp + stats.Shield - Dmg;
                    stats.Shield = 0;
                }
                else
                {
                    stats.Shield = stats.Shield - Dmg;
                }
                if (stats.Hp <= 0)
                {
                    Destroy(Instantiate(bigExplosion, collision.contacts[0].point, Quaternion.identity), 2);
                    OwnerStats.Score += stats.Score;
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
