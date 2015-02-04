using UnityEngine;
using System.Collections;

public class aiFtEnemy : MonoBehaviour
{

    private motionEnemy motionEnemy = null;
    public weaponSmallRockets weapon;
    public GameObject rotorR;
    public GameObject rotorL;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
            weapon.Fire(gameData.playerPosition );
    }

    private void ThinkChangeHeight()
    {
        if (motionEnemy.moveVertical == 0 && Random.Range(0, 100) % (12 / (int)gameData.difficulty) == 0)
        {
            float tmp = transform.position.y - gameData.playerPosition.y;
            if (tmp < -0.15 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical -= tmp;
            }
            else if (tmp > 0.15 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical -= tmp;
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
                motionEnemy.moveHorizontal += Random.Range(-tmp / 2, -3 * tmp);
            }
            else if (tmp > 0)
            {
                motionEnemy.moveHorizontal -= tmp;
                motionEnemy.moveHorizontal -= Random.Range(tmp / 2, 3 * tmp);
            }
        }
    }
    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            ThinkMove();
            ThinkFire();
            ThinkChangeHeight();
            if (rotorL != null)
                rotorL.gameObject.transform.Rotate(new Vector3(0, 1, 0), 120*Time.deltaTime, Space.World);
            if (rotorR != null)
                rotorR.gameObject.transform.Rotate(new Vector3(0, 1, 0), -120*Time.deltaTime, Space.World);
        }
    }
}
