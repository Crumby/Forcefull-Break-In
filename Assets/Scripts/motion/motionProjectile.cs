using UnityEngine;
using System.Collections;

public class motionProjectile : MonoBehaviour
{

    [Range(0.0F, 500.0F)]
    public float forwardSpeed, destroyDmg;
    public Vector3 directionVector { get; set; }
    [HideInInspector]
    public bool isPlayers = false, launch = false;

    void Start()
    {
        directionVector = Vector3.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPlayers && collision.gameObject.GetComponent<shipSystemsEnemy>() != null && transform.parent == null)
        {
            var enemy = collision.gameObject.GetComponent<shipSystemsEnemy>();
            if (enemy != null)
            {
                gameData.addPower = 5;
                if (enemy.recieveDmg(destroyDmg, collider.bounds.max))
                    gameData.addScore = enemy.score;
                Destroy(gameObject);
            }
        }
        else if (isPlayers && collision.gameObject.GetComponent<basicEnemySystems>() != null && transform.parent == null)
        {
            var enemy = collision.gameObject.GetComponent<basicEnemySystems>();
            if (enemy != null)
            {
                gameData.addPower = 5;
                if (enemy.recieveDmg(destroyDmg, collider.bounds.max))
                    gameData.addScore = enemy.score;
                Destroy(gameObject);
            }
        }
        else if (!isPlayers && collision.gameObject.GetComponent<shipSystemsPlayer>() != null && transform.parent == null)
        {
            var player = collision.gameObject.GetComponent<shipSystemsPlayer>();
            if (player != null)
            {
                player.recieveDmg(destroyDmg, collider.bounds.min);
                Destroy(gameObject);
            }
            else if (gameData.isChildOfPlayer(collision.gameObject.transform))
            {
                player = collision.gameObject.transform.parent.GetComponent<shipSystemsPlayer>();
                player.recieveDmg(destroyDmg, collider.bounds.min);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.GetComponent<aiMeteor>() != null)
        {
            var tmp = (aiMeteor)collision.gameObject.GetComponent<aiMeteor>();
            tmp.recieveDmg(destroyDmg, transform.position);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && launch)
        {
            transform.Translate(directionVector * forwardSpeed * Time.deltaTime);
            if (gameData.gameBounds != null)
                if (transform.position.x > gameData.gameBounds.collider.bounds.max.x) Destroy(gameObject);
                else if (transform.position.x < gameData.gameBounds.collider.bounds.min.x) Destroy(gameObject);
                else if (transform.position.y > gameData.gameBounds.collider.bounds.max.y) Destroy(gameObject);
                else if (transform.position.y < gameData.gameBounds.collider.bounds.min.y) Destroy(gameObject);
                else if (transform.position.z > gameData.playerPosition.z + gameData.aiActivation) Destroy(gameObject);
                else if (transform.position.z <= gameData.cameraOffsite.z + gameData.playerPosition.z) Destroy(gameObject);
        }
    }
}
