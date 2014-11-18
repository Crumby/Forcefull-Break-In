using UnityEngine;
using System.Collections;

public class motionPlayer : MonoBehaviour
{

    [Range(0.0F, 100.0F)]
    public float forwardSpeed, horizontalSpeed, verticalSpeed;
    [Range(0.0F, 60.0F)]
    public float horizontalRotation, maxAcceleration;
	public Transform movingObject;
    private float acceleration;

    // Use this for initialization
    void Start()
    {
        gameData.initPlayer(this);
    }

    private void moveRight(float speed)
    {
        if (collider.bounds.max.x + speed * Time.deltaTime < gameData.gameBounds.collider.bounds.max.x && acceleration >= 0 && movingObject.rotation.z <= 0.001f)
        {
			movingObject.Translate(speed * Time.deltaTime, 0, 0, Space.World);
            if (acceleration < maxAcceleration)
                acceleration += speed * Time.deltaTime;
            if (Camera.main.transform.rotation.eulerAngles.z > 180 || Camera.main.transform.rotation.eulerAngles.z <= 0.001f)
            {
                Camera.main.transform.Rotate(0, 0, -horizontalSpeed * Time.deltaTime, Space.World);
                if (Camera.main.transform.rotation.eulerAngles.z < 360 - horizontalRotation)
                    Camera.main.transform.rotation = Quaternion.Euler(0, 0, -horizontalRotation);
            }
        }
        else { balanceSides(); accelerateSides(); }
    }

    private void moveLeft(float speed)
    {
        if (collider.bounds.min.x - speed * Time.deltaTime > gameData.gameBounds.collider.bounds.min.x && acceleration <= 0 && movingObject.rotation.z >= -0.001f)
        {
            movingObject.Translate(-speed * Time.deltaTime, 0, 0, Space.World);
            if (acceleration > -maxAcceleration)
                acceleration -= speed * Time.deltaTime;
            if (Camera.main.transform.rotation.eulerAngles.z < horizontalRotation)
            {
                Camera.main.transform.Rotate(0, 0, horizontalSpeed * Time.deltaTime, Space.World);
                if (Camera.main.transform.rotation.eulerAngles.z > horizontalRotation)
                    Camera.main.transform.rotation = Quaternion.Euler(0, 0, horizontalRotation);
            }
        }
        else { balanceSides(); accelerateSides(); }

    }

    private void accelerateSides()
    {
        if (acceleration > 0)
        {
            movingObject.Translate(horizontalSpeed * Time.deltaTime, 0, 0, Space.World);
            acceleration -= horizontalSpeed * Time.deltaTime;
            if (acceleration < 0)
                acceleration = 0;
        }
        else if (acceleration < 0)
        {
            movingObject.Translate(-horizontalSpeed * Time.deltaTime, 0, 0, Space.World);
            acceleration += horizontalSpeed * Time.deltaTime;
            if (acceleration > 0)
                acceleration = 0;
        }
    }

    private void balanceSides()
    {
        if (Camera.main.transform.rotation.eulerAngles.z > 180)
        {
            Camera.main.transform.Rotate(0, 0, 2 * horizontalSpeed * Time.deltaTime, Space.World);
            if (Camera.main.transform.rotation.eulerAngles.z < 180)
                Camera.main.transform.rotation = Quaternion.identity;
        }
        else if (Camera.main.transform.rotation.eulerAngles.z > 0.001f)
        {
            Camera.main.transform.Rotate(0, 0, -2 * horizontalSpeed * Time.deltaTime, Space.World);
            if (Camera.main.transform.rotation.eulerAngles.z > 180)
                Camera.main.transform.rotation = Quaternion.identity;
        }
    }

    private void moveUp(float speed)
    {
        if (collider.bounds.max.y + speed * Time.deltaTime < gameData.gameBounds.collider.bounds.max.y)
            movingObject.Translate(0, speed * Time.deltaTime, 0, Space.World);

    }

    private void moveDown(float speed)
    {
        if (collider.bounds.min.y - speed * Time.deltaTime > gameData.gameBounds.collider.bounds.min.y)
            movingObject.Translate(0, -speed * Time.deltaTime, 0, Space.World);
    }

    // Update is called once per frame
    void Update()
    {   
        if (!gameData.pausedGame)
        {
            if (movingObject.position.z > gameData.gameBounds.collider.bounds.min.z && movingObject.position.z < gameData.gameBounds.collider.bounds.max.z)
                movingObject.Translate(0, 0, forwardSpeed * Time.deltaTime, Space.World);
            if (Input.GetButton("Horizontal"))
            {
                if (Input.GetAxis("Horizontal") > 0) moveRight(horizontalSpeed);
                else if (Input.GetAxis("Horizontal") < 0) moveLeft(horizontalSpeed);
            }
            else
            {
                balanceSides();
                accelerateSides();
            }
            if (Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") > 0) moveUp(horizontalSpeed);
                else if (Input.GetAxis("Vertical") < 0) moveDown(horizontalSpeed);
            }
        }
    }
}
