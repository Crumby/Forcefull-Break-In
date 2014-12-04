using UnityEngine;
using System.Collections;

public class ProjectileExpiredExplosion : MonoBehaviour
{

    public GameObject explosion;
    public float timer;
    private float createTime = 0.0f;

    void Start()
    {
        createTime = Time.time;
    }

    void Update()
    {
        if (!gameData.pausedGame)
        {
            if ((Time.time - createTime) > timer)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}