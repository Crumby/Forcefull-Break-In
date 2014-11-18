using UnityEngine;
using System.Collections;

public class motionProjectile : MonoBehaviour
{

    [Range(0.0F, 100.0F)]
    public float forwardSpeed, destroyDmg;
	public Vector3 directionVector { get; set; }
    public Vector3 destinationPoint { get; set; }
    public bool isPlayers { get; set; }
    private bool p_launch;
    public bool launch { 
        get { return p_launch; }
        set { p_launch = value;
        if (p_launch && destinationPoint != Vector3.zero)
            transform.LookAt(destinationPoint,Vector3.up);    
        }
    }
	
	void Start(){
		directionVector=Vector3.forward;
        destinationPoint = Vector3.zero;
        p_launch = false;
	}
    
    void OnCollisionEnter(Collision collision)
    {
        if (launch)
        {
            if (isPlayers && collision.gameObject.GetComponent<motionPlayer>() == null)
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
            else if (!isPlayers && collision.gameObject.GetComponent<motionEnemy>() == null)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame&&launch)
        {
			transform.Translate(directionVector * forwardSpeed * Time.deltaTime);
			if (transform.position.x > gameData.playerPosition.x + gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.x < -gameData.playerPosition.x - gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.y > gameData.playerPosition.y + gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.y < -gameData.playerPosition.y - gameData.aiActivation) Destroy(gameObject);
			else if (transform.position.z > gameData.playerPosition.z + gameData.aiActivation) Destroy(gameObject);
            else if (transform.position.z <= gameData.cameraOffsite.z + gameData.playerPosition.z) Destroy(gameObject);
        }
    }
}
