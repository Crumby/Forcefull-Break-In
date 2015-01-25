using UnityEngine;
using System.Collections;

public class basicEnemySystems : MonoBehaviour
{

    [Range(0.0F, 500.0F)]
    public float collisionDmg;
    [Range(0.0F, 500.0F)]
    public int maxHealth, score;
    public int health { get; private set; }
    public GameObject smallExplosion, fires, destroyExplosion;
    public Transform[] endEplosionPos;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (dmg >= health && health > 0)
            destroyEnemy();
        health -= Mathf.CeilToInt(dmg);
        return health > 0;
    }

    private void destroyEnemy()
    {
        Instantiate(destroyExplosion, transform.position, Quaternion.identity);
        foreach (var where in endEplosionPos) { 
            var tmp=(GameObject)Instantiate(fires, where.position, Quaternion.identity);
            tmp.transform.parent = transform;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<shipSystemsPlayer>();
        if (enemy != null)
        {
            enemy.recieveDmg(collisionDmg, collision.contacts[0].point);
        }
    }

    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (transform.position.z <= gameData.playerPosition.z + gameData.cameraOffsite.z)
                Destroy(gameObject);
        }
    }
}
