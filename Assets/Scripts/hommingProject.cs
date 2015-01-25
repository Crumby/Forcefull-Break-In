using UnityEngine;
using System.Collections;

public class hommingProject : MonoBehaviour
{

    [Range(0.0F, 500.0F)]
    public float forwardSpeed, destroyDmg;
    [HideInInspector]
    public bool isPlayers = false;
    private float timer = 0;
    public float destroyTime = 8;
    public bool launch = false;

    void OnCollisionEnter(Collision collision)
    {
        if (isPlayers && collision.gameObject.GetComponent<shipSystemsEnemy>() != null && transform.parent == null)
        {
            var enemy = collision.gameObject.GetComponent<shipSystemsEnemy>();
            if (enemy != null)
            {
                gameData.addPower = 5;
                if (enemy.recieveDmg(destroyDmg, collider.bounds.max))
                    gameData.score = enemy.score;
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
                    gameData.score = enemy.score;
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
            if (timer >= destroyTime)
                Destroy(gameObject);
            else
                timer += Time.deltaTime;
            if (gameData.playerMotion != null)
            {
                var target = gameData.playerMotion.gameObject;
                if (target != null)
                {
                    var relativePos = target.transform.position - transform.position;
                    var rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.035f);
                }
                transform.Translate(0, 0, forwardSpeed * Time.deltaTime, Space.Self);
            }
            if (gameData.gameBounds != null)
                if (transform.position.y > gameData.gameBounds.collider.bounds.max.y) Destroy(gameObject);
                else if (transform.position.y < gameData.gameBounds.collider.bounds.min.y) Destroy(gameObject);
        }
    }
}
