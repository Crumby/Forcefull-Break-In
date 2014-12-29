using UnityEngine;
using System.Collections;

public class aiSpiner : MonoBehaviour
{
    [Range(0.0F, 720.0F)]
    public float rotationSpeed;
    public weaponEnergyBlaster[] blasters;
    private motionEnemy motionEnemy = null;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
        {
            int blster = 0;
            for (int i = 0; i < blasters.Length; i++)
                if (Vector3.Distance(blasters[i].transform.position, gameData.playerPosition) < Vector3.Distance(blasters[blster].transform.position, gameData.playerPosition))
                    blster = i;
            if(Random.Range(0,25)%5==0)
                blasters[blster].Fire(gameData.playerPosition+Vector3.forward*Time.deltaTime*gameData.forwardSpeed);
            else
                blasters[blster].Fire(gameData.playerPosition);
        }
    }

    private void ThinkChangeHeight()
    {
        if (motionEnemy.moveVertical == 0 && Random.Range(0, 100) % (12 / (int)gameData.difficulty) == 0)
        {
            float tmp = transform.position.y - gameData.playerPosition.y;
            if (tmp < -0.25 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical -= tmp;
                if (Random.Range(0, 50) % 4 == 0)
                    motionEnemy.moveVertical += Random.Range(-tmp / 2, -3 * tmp);
            }
            else if (tmp > 0.25 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical -= tmp;
                if (Random.Range(0, 50) % 4 == 0)
                    motionEnemy.moveVertical -= Random.Range(tmp / 2, 3 * tmp);
            }

        }
    }

    private void ThinkMove()
    {
        if (motionEnemy.moveHorizontal == 0 && Random.Range(0, 50) % (12 / (int)gameData.difficulty) == 0)
        {
            float tmp = transform.position.x - gameData.playerPosition.x;
            if (tmp < 0)
            {
                motionEnemy.moveHorizontal -= tmp;
            }
            else if (tmp > 0)
            {
                motionEnemy.moveHorizontal -= tmp;
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
