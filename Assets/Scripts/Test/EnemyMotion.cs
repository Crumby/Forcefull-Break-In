using UnityEngine;
using System.Collections;

public class EnemyMotion : MonoBehaviour
{
    public float speed;
    public int CollisionDmg = 25;

    void Update()
    {
        if (!PlayerMotion.Pause)
        {
            transform.Translate(Vector3.back * speed);
            if (transform.position.z < -400)
                Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerMotion>() != null)
        {
            var stats = collision.gameObject.GetComponent<Stats>();
            if (stats != null)
            {
                if (stats.Shield - CollisionDmg <= 0)
                {
                    stats.Hp = stats.Hp + stats.Shield - CollisionDmg;
                    stats.Shield = 0;
                }
                else
                {
                    stats.Shield = stats.Shield - CollisionDmg;
                }
                if (stats.Hp <= 0)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
        else
            Destroy(collision.gameObject);
    }

}
