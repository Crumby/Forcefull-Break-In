using UnityEngine;
using System.Collections;

public class aiMixer : MonoBehaviour
{
    [Range(0.0F, 720.0F)]
    public float fireSpeed;
    public weaponBlastCanon[] weapons;
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
            weapons[weapon].Fire(gameData.playerPosition);
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
            if (tmp < -0.25 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical += tmp;
            }
            else if (tmp > 0.25 * gameData.gameBounds.collider.bounds.size.y)
            {
                motionEnemy.moveVertical += tmp;
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
            if (transform.rotation.eulerAngles.y < fireRotation)
            {
                transform.Rotate(0, fireSpeed * Time.deltaTime, 0, Space.Self);
                if (transform.rotation.eulerAngles.y > fireRotation)
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, fireSpeed, transform.rotation.eulerAngles.z);
            }
        }
    }
}
