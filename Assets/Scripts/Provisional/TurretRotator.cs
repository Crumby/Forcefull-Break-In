using UnityEngine;
using System.Collections;

public class TurretRotator : MonoBehaviour
{

    public GameObject target;

    void Update()
    {
        if (!gameData.pausedGame && GetComponentInParent<basicEnemySystems>() != null && GetComponentInParent<basicEnemySystems>().health > 0)
        {
            target = GameObject.FindGameObjectWithTag("BullsEye");
            if (target != null)
                transform.LookAt(target.transform);
        }
    }
}