using UnityEngine;
using System.Collections;

public class motionEnemy : MonoBehaviour
{
    [Range(0.0F, 100.0F)]
    public float forwardSpeed, horizontalSpeed, verticalSpeed;
    [Range(0.0F, 60.0F)]
    public float horizontalRotation;
    public float moveHorizontal { get; set; }
    public float moveVertical { get; set; }

    void Start()
    {
        moveVertical = 0;
        moveHorizontal = 0;
    }

    private void moveRight(float speed)
    {
        if (collider.bounds.max.x + speed * Time.deltaTime < gameData.gameBounds.collider.bounds.max.x && transform.rotation.z <= 0.001f)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
            moveHorizontal -= speed * Time.deltaTime;
            if (moveHorizontal < 0) moveHorizontal = 0;
            if (transform.rotation.eulerAngles.z > 180 || transform.rotation.eulerAngles.z == 0.001f)
            {
                transform.Rotate(0, 0, -horizontalSpeed * Time.deltaTime, Space.Self);
                if (transform.rotation.eulerAngles.z < 360 - horizontalRotation)
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -horizontalRotation);
            }
        }
        else
        {
            moveHorizontal = 0;
            balanceSides();
        }
    }

    private void moveLeft(float speed)
    {
        if (collider.bounds.min.x - speed * Time.deltaTime > gameData.gameBounds.collider.bounds.min.x && transform.rotation.z >= -0.001f)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
            moveHorizontal += speed * Time.deltaTime;
            if (moveHorizontal > 0) moveHorizontal = 0;
            if (transform.rotation.eulerAngles.z < horizontalRotation)
            {
                transform.Rotate(0, 0, horizontalSpeed * Time.deltaTime, Space.Self);
                if (transform.rotation.eulerAngles.z > horizontalRotation)
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, horizontalRotation);
            }
        }
        else
        {
            moveHorizontal = 0;
            balanceSides();
        }

    }

    private void balanceSides()
    {
        if (transform.rotation.eulerAngles.z > 180)
        {
            transform.Rotate(0, 0, horizontalSpeed * Time.deltaTime, Space.Self);
            if (transform.rotation.eulerAngles.z < 180)
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
        else if (transform.rotation.eulerAngles.z > 0.001f)
        {
            transform.Rotate(0, 0, -horizontalSpeed * Time.deltaTime, Space.Self);
            if (transform.rotation.eulerAngles.z > 180)
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }

    private void moveUp(float speed)
    {
        if (collider.bounds.max.y + speed * Time.deltaTime < gameData.gameBounds.collider.bounds.max.y && transform.rotation.x <= 0)
        {
            transform.Translate(0, speed * Time.deltaTime, 0, Space.World);
            moveVertical -= speed * Time.deltaTime;
            if (moveVertical < 0) moveVertical = 0;
        }
        else moveVertical = 0;

    }

    private void moveDown(float speed)
    {
        if (collider.bounds.min.y - speed * Time.deltaTime > gameData.gameBounds.collider.bounds.min.y && transform.rotation.x >= 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0, Space.World);
            moveVertical += speed * Time.deltaTime;
            if (moveVertical > 0) moveVertical = 0;
        }
        else moveVertical = 0;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<motionEnemy>() != null)
        {
            moveHorizontal = -2 * moveHorizontal;
            moveVertical = -2 * moveVertical;
        }
    }

    void Update()
    {
        if (transform.position.z < gameData.cameraOffsite.z + gameData.playerPosition.z)
        {
            DestroyObject(gameObject);
        }
        else if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            transform.Translate(0, 0, -forwardSpeed * Time.deltaTime, Space.World);
            if (moveHorizontal != 0)
            {
                if (moveHorizontal > 0) moveRight(horizontalSpeed);
                else if (moveHorizontal < 0) moveLeft(horizontalSpeed);
            }
            else balanceSides();
            if (moveVertical != 0)
            {
                if (moveVertical > 0) moveUp(horizontalSpeed);
                else if (moveVertical < 0) moveDown(horizontalSpeed);
            }
        }
    }
}
