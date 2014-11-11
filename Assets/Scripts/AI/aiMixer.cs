using UnityEngine;
using System.Collections;

public class aiMixer : MonoBehaviour
{
    [Range(0.0F, 720.0F)]
    public float fireSpeed;
    public weaponRocketLaucher[] weapons;
    private motionEnemy motionEnemy = null;
    private float fireRotation = 0;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (transform.rotation.eulerAngles.y < fireRotation && Random.Range(0, 150) % 50 == 0)
        {
            int weapon = 0;
            for (int i = 0; i < weapons.Length; i++)
                if (Vector3.Distance(weapons[i].transform.position, gameData.playerPosition) < Vector3.Distance(weapons[weapon].transform.position, gameData.playerPosition))
                    weapon = i;
            weapons[weapon].rotation = Quaternion.LookRotation(Vector3.Normalize(gameData.playerPosition - weapons[weapon].transform.position));
            weapons[weapon].Fire(Vector3.forward, false);
            transform.Rotate(0, fireSpeed * Time.deltaTime, 0, Space.Self);
            if (transform.rotation.eulerAngles.y > fireRotation)
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, fireSpeed, transform.rotation.eulerAngles.z);
        }
        else if (Random.Range(0, 100) % 80 == 0)
        {
            if (fireRotation + 90 == 360)
                fireRotation = 360;
            else fireRotation = (fireRotation + 90) % 360;
        }

    }

    private void ThinkChangeHeight()
    {
        if (motionEnemy.moveVertical == 0 && Random.Range(0, 50) % 3 == 0)
        {
            float tmp = transform.position.y - gameData.playerPosition.y;
            if (tmp < 0)
            {
                motionEnemy.moveVertical -= tmp;
                //if (Random.Range(0, 50) % 4 == 0)
                //    motionEnemy.moveVertical += Random.Range(-tmp / 2, -3 * tmp);
            }
            else if (tmp > 0)
            {
                motionEnemy.moveVertical -= tmp;
                //if (Random.Range(0, 50) % 4 == 0)
                //    motionEnemy.moveVertical -= Random.Range(tmp / 2, 3 * tmp);
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
                motionEnemy.moveHorizontal += Random.Range(-tmp / 2, -5 * tmp);
            }
            else if (tmp > 0)
            {
                motionEnemy.moveHorizontal -= Random.Range(tmp / 2, 5 * tmp);
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
