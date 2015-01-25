﻿using UnityEngine;
using System.Collections;

public class weaponBoatTurret : MonoBehaviour
{

    public GameObject projectile;
    public Transform where;

    public void Fire(Vector3 to)
    {
        var tmp_1 = (GameObject)Instantiate(projectile, where.position, Quaternion.identity);
        var tmp = tmp_1.GetComponent<motionProjectile>();
        tmp.transform.LookAt(to, Vector3.up);
        tmp.forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
        tmp.launch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            transform.LookAt(gameData.playerPosition, Vector3.up);
        }
    }
}
