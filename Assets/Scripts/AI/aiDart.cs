using UnityEngine;
using System.Collections;

public class aiDart : MonoBehaviour
{

    private motionEnemy motionEnemy = null;
    public weaponCann weapon;

    void Start()
    {
        motionEnemy = GetComponent<motionEnemy>();
    }

    private void ThinkFire()
    {
        if (Random.Range(0, 150) % (60 / (int)gameData.difficulty) == 0)
            weapon.Fire();
    }

    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            ThinkFire();
        }
    }

    //lode naskriptuj
    //koec lvl1
    //gameprefab do all scenes
    //lvl2 bos scripty
}
