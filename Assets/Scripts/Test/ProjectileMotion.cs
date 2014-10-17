using UnityEngine;
using System.Collections;

public class ProjectileMotion : MonoBehaviour
{
    private float timeToLive = 5;
    public int speed = 50;
    public int Dmg = 50;
    public Stats OwnerStats;
    public GameObject explosion;

    void Update()
    {
        if (!PlayerMotion.Pause)
        {
            timeToLive -= Time.deltaTime;
            transform.Translate(Vector3.forward * speed);
        }
        if (timeToLive <= 0)
            Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var ic = Instantiate(explosion, collision.contacts[0].point, Quaternion.identity);
        Destroy(ic, 2);
        if (collision.gameObject.GetComponent<Stats>() != OwnerStats)
        {
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
                    OwnerStats.Score += stats.Score;
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
