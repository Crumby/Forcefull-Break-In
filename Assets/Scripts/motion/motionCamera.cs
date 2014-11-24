using UnityEngine;
using System.Collections;

public class motionCamera : MonoBehaviour
{

    [Range(0.0F, 50.0F)]
    public float horizontalMargin, verticalMargin;
    // Use this for initialization
    void Start()
    {
        gameData.initCamera(this);
    }

    private void moveRight(float speed)
    {
        if (transform.position.x + horizontalMargin - gameData.cameraOffsite.x < gameData.playerPosition.x)
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
    }
    private void moveLeft(float speed)
    {
        if (transform.position.x - horizontalMargin - gameData.cameraOffsite.x > gameData.playerPosition.x)
            transform.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
    }

    private void moveUp(float speed)
    {
        if (transform.position.y + verticalMargin - gameData.cameraOffsite.y < gameData.playerPosition.y)
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
    }
    private void moveDown(float speed)
    {
        if (transform.position.y - gameData.cameraOffsite.y > gameData.playerPosition.y)
            transform.Translate(0, -speed * Time.deltaTime, 0, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (gameData.gameBounds.collider.bounds.max.z - gameData.endOffsite > transform.position.z - gameData.cameraOffsite.z)
                transform.Translate(Vector3.forward * gameData.forwardSpeed * Time.deltaTime, Space.World);
            moveLeft(gameData.horizontalSpeed);
            moveRight(gameData.horizontalSpeed);
            moveUp(gameData.verticalSpeed);
            moveDown(gameData.verticalSpeed);
        }
    }
}
