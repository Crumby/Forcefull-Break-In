using UnityEngine;
using System.Collections;

public class aiBoat : MonoBehaviour
{

    private motionEnemy motionEnemy = null;
    public weaponBoatTurret[] weapons;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
            weapons[Random.Range(0, weapons.Length)].Fire(gameData.playerPosition+new Vector3(0,0,25));
    }


    private void ThinkMove()
    {
        if (motionEnemy.moveHorizontal == 0 && Random.Range(0, 50) % (12 / (int)gameData.difficulty) == 0)
        {
            float tmp = (transform.position.x - gameData.playerPosition.x)%150;
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
        if (!gameData.pausedGame && gameData.inReach(transform.position) && GetComponent<basicEnemySystems>() != null && GetComponent<basicEnemySystems>().health > 0)
        {
            ThinkMove();
            ThinkFire();
        }
    }
}
