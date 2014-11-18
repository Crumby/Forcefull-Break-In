using UnityEngine;
using System.Collections;

public class weaponPower_0 : MonoBehaviour
{

    [Range(0.0F, 500.0F)]
    public float forwardSpeed;
    [Range(0.0F, 500.0F)]
    public int hitScore;
    [HideInInspector]

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<motionEnemy>() != null||
            collision.gameObject.GetComponent<motionProjectile>() != null||
            collision.gameObject.GetComponent<aiMeteor>() != null)
        {
            Destroy(collision.gameObject);
            gameData.addScore = hitScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
            if (transform.position.z > gameData.playerPosition.z + gameData.aiActivation)
                Destroy(gameObject);
        }
    }
}
