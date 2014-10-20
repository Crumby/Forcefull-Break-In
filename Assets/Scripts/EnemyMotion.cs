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
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
            if (transform.position.z < -400)
                Destroy(this.gameObject);
            if (transform.position.x > PlayerMotion.MaxTracksX || transform.position.x < PlayerMotion.MinTracksX)
                Destroy(this.gameObject, 3);
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
