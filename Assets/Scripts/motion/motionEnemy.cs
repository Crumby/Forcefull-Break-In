﻿using UnityEngine;
using System.Collections;

public class motionEnemy : MonoBehaviour
{
    [Range(0.0F, 100.0F)]
    public float forwardSpeed, horizontalSpeed, verticalSpeed, waitOffset;
    [Range(0.0F, 60.0F)]
    public float horizontalRotation;
    [Range(0.0F, 50.0F)]
    public float waitInFrontPlayer, colisionWait;
    public float moveHorizontal { get; set; }
    public float moveVertical { get; set; }
    public EnemyCollision colided { get; private set; }
    private float timerColide, timerForPlayer;

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
            moveHorizontal = -moveHorizontal / 2;
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
            moveHorizontal = -moveHorizontal / 2;
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
        else moveVertical = -moveVertical / 2;

    }

    private void moveDown(float speed)
    {
        if (collider.bounds.min.y - speed * Time.deltaTime > gameData.gameBounds.collider.bounds.min.y && transform.rotation.x >= 0)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0, Space.World);
            moveVertical += speed * Time.deltaTime;
            if (moveVertical > 0) moveVertical = 0;
        }
        else moveVertical = -moveVertical / 2;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<motionEnemy>() != null)
        {
            Vector3 point = collision.contacts[0].point;
            if (point.x > transform.position.x)
                colided = EnemyCollision.RIGTH;
            else if (point.x < transform.position.x)
                colided = EnemyCollision.LEFT;
            else if (point.z < transform.position.z)
                colided = EnemyCollision.FRONT;
            else if (point.z < transform.position.z)
                colided = EnemyCollision.BACK;
            else if (point.y < transform.position.y)
                colided = EnemyCollision.DOWN;
            else if (point.y < transform.position.y)
                colided = EnemyCollision.UP;
            //Debug.Log("COLISION");
        }
        else
        {
            colided = EnemyCollision.NONE;
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
            if (colided != EnemyCollision.FRONT)
            {
                if (transform.position.z > waitOffset + gameData.playerPosition.z)
                    transform.Translate(0, 0, -forwardSpeed * Time.deltaTime, Space.World);
                else
                    transform.Translate(0, 0, gameData.forwardSpeed * Time.deltaTime, Space.World);
            }
            if (transform.position.z > waitOffset + gameData.playerPosition.z)
                timerForPlayer += Time.deltaTime;
            if (timerForPlayer > waitInFrontPlayer)
                waitOffset = float.MinValue;
            if (moveHorizontal != 0)
            {
                if (moveHorizontal > 0 && colided != EnemyCollision.RIGTH) moveRight(horizontalSpeed);
                else if (moveHorizontal < 0 && colided != EnemyCollision.LEFT) moveLeft(horizontalSpeed);
            }
            else balanceSides();
            if (moveVertical != 0)
            {
                if (moveVertical > 0 && colided != EnemyCollision.UP) moveUp(horizontalSpeed);
                else if (moveVertical < 0 && colided != EnemyCollision.DOWN) moveDown(horizontalSpeed);
            }
            if (colided != EnemyCollision.NONE)
            {
                timerColide += Time.deltaTime;
                if (timerColide > colisionWait)
                {
                    colided = EnemyCollision.NONE;
                    timerColide = 0;
                }
            }
        }
    }
}
