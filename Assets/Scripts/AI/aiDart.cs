using UnityEngine;
using System.Collections;

public class aiDart : MonoBehaviour
{

    public weaponCann weapon;

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
            weapon.Fire();
    }

    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
            ThinkFire();
    }
}
