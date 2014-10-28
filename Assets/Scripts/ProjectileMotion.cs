using UnityEngine;
using System.Collections;

public class ProjectileMotion : MonoBehaviour
{
    private float timeToLive = 5;
    public int speed = 500;
    public int Dmg = 50;
    public Stats OwnerStats;
    public GameObject explosion, bigExplosion;
    public Vector3 forward = Vector3.forward;

    void Update()
    {
        if (!GameData.PauseGame)
        {
            timeToLive -= Time.deltaTime;
            transform.Translate(forward * speed * Time.deltaTime);
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
                    if (collision.gameObject.GetComponent<PlayerMotion>() != null)
                        GameObject.Find("Canvas").GetComponent<InGameMenu>().GameOver();
                    else
                        Destroy(collision.gameObject);
                }
            }
        }
    }
}
