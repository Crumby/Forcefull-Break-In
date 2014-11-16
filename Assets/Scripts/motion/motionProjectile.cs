using UnityEngine;
using System.Collections;

public class motionProjectile : MonoBehaviour
{

    [Range(0.0F, 100.0F)]
    public float forwardSpeed, destroyDmg;
    public Vector3 direction { get; set; }
    public bool isPlayers { get; set; }

    void OnCollisionEnter(Collision collision)
    {
        if (isPlayers)
        {
            var enemy = collision.gameObject.GetComponent<shipSystemsEnemy>();
            if (enemy != null)
            {
				gameData.addPower=5;
                if (enemy.recieveDmg(destroyDmg, collider.bounds.max))
                    gameData.addScore = enemy.score;
                Destroy(gameObject);
            }
        }
        else
        {
            var player = collision.gameObject.GetComponent<shipSystemsPlayer>();
            if (player != null)
            {
                player.recieveDmg(destroyDmg, collision.contacts[0].point);
                Destroy(gameObject);
            }
            else if (gameData.isChildOfPlayer(collision.gameObject.transform))
            {
                player = collision.gameObject.transform.parent.GetComponent<shipSystemsPlayer>();
                player.recieveDmg(destroyDmg, collision.contacts[0].point);
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            transform.Translate(direction * forwardSpeed * Time.deltaTime);
			if (transform.position.x > gameData.playerPosition.x + gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.x < -gameData.playerPosition.x - gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.y > gameData.playerPosition.y + gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.y < -gameData.playerPosition.y - gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.z > gameData.playerPosition.z + gameData.aiActivation) Destroy(gameObject);
            else if (transform.position.z <= gameData.cameraOffsite.z + gameData.playerPosition.z) Destroy(gameObject);
        }
    }
}
