using UnityEngine;
using System.Collections;

public class aiDart : MonoBehaviour {

    private motionEnemy motionEnemy = null;
    public weaponCann weapon;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % 50 == 0)
        {
            weapon.Fire();
        }
    }

    private void ThinkChangeHeight()
    {
        if (motionEnemy.moveVertical == 0 && Random.Range(0, 50) % 3 == 0)
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
        if (motionEnemy.moveHorizontal == 0 && Random.Range(0, 50) % 5 == 0)
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
        }
    }
}
