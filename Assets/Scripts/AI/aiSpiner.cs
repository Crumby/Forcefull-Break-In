﻿using UnityEngine;
using System.Collections;

public class aiSpiner : MonoBehaviour
{
    [Range(0.0F, 720.0F)]
    public float rotationSpeed;
    private motionEnemy motionEnemy = null;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {

    }

    private void ThinkChangeHeight()
    {
        if (motionEnemy.moveVertical == 0 && Random.Range(0, 50) % 3 == 0)
        {
            float tmp = transform.position.y - gameData.playerPosition.y;
            if (tmp < 0)
            {
                motionEnemy.moveVertical -= tmp;
                if (Random.Range(0, 50) % 4 == 0)
                    motionEnemy.moveVertical += Random.Range(-tmp / 2, -3*tmp);
            }
            else if (tmp > 0)
            {
                motionEnemy.moveVertical -= tmp;
                if (Random.Range(0, 50) % 4 == 0)
                    motionEnemy.moveVertical -= Random.Range(tmp / 2, 3*tmp);
            }

        }
    }

    private void ThinkMove()
    {
        if (motionEnemy.moveHorizontal == 0 && Random.Range(0, 50) % 5 == 0)
        {
            float tmp = transform.position.x - gameData.playerPosition.x;
            if (tmp < 0)
            {
                motionEnemy.moveHorizontal -= tmp;
                //motionEnemy.moveHorizontal += Random.Range(-tmp / 2, -3*tmp);
            }
            else if (tmp > 0)
            {
                motionEnemy.moveHorizontal -= tmp;
                //motionEnemy.moveHorizontal -= Random.Range(tmp / 2, 3*tmp);
            }
        }
    }

    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
            ThinkMove();
            ThinkFire();
            ThinkChangeHeight();
        }
    }
}
