using UnityEngine;
using System.Collections;

public class Lvl2BossTurretRotator : MonoBehaviour
{

    private GameObject target;

    void Update()
    {
        if (!gameData.pausedGame && GetComponentInParent<basicEnemySystems>() != null && GetComponentInParent<basicEnemySystems>().health > 0)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            transform.LookAt(target.transform);
        }
    }
}