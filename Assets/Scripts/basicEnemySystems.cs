using UnityEngine;
using System.Collections;

public class basicEnemySystems : MonoBehaviour
{

    [Range(0.0F, 500.0F)]
    public float collisionDmg;
    [Range(0.0F, 500.0F)]
    public int maxHealth, score;
    public int health { get; set; }
    public GameObject smallExplosion, fires, destroyExplosion;
    public Transform[] endEplosionPos;
    [HideInInspector]
    public bool imortal = false;

    // Use this for initialization
    void Start()
    {
        health = maxHealth * (int)gameData.difficulty;
    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (!imortal)
        {
            if (dmg >= health && health > 0)
                destroyEnemy();
            health -= Mathf.CeilToInt(dmg);
        }
        return health > 0;
    }

    private void destroyEnemy()
    {
        Instantiate(destroyExplosion, transform.position, Quaternion.identity);
        foreach (var where in endEplosionPos)
        {
            var tmp = (GameObject)Instantiate(fires, where.position, Quaternion.identity);
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
        else if (collision.gameObject.GetComponent<TerrainCollider>() != null && GetComponent<motionEnemy>() != null) destroyEnemy();
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
