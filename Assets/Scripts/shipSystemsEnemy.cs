﻿using UnityEngine;
using System.Collections;

public class shipSystemsEnemy : MonoBehaviour
{
    [Range(0.0F, 500.0F)]
    public float shieldRegen, collisionDmg;
    [Range(0.0F, 500.0F)]
    public int maxHealth, maxShield, score;
    public int health { get; private set; }
    public int shield { get; private set; }
    public GameObject smallExplosion, bigExplosion;

    // Use this for initialization
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
    }

    private void shieldRegeneration()
    {
        if (shield < maxShield)
            if (Mathf.CeilToInt(shieldRegen * Time.deltaTime) + shield > maxShield)
                shield = maxShield;
            else shield += Mathf.CeilToInt(shieldRegen * Time.deltaTime);

    }

    public bool recieveDmg(float dmg, Vector3 where)
    {
        Instantiate(smallExplosion, where, Quaternion.identity);
        if (shield - dmg <= 0)
        {
            shield = 0;
            health += Mathf.CeilToInt(shield - dmg);
            return health <= 0;
        }
        else shield -= Mathf.CeilToInt(dmg);
        return false;
    }

    private void destroyShip()
    {
        Instantiate(bigExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<shipSystemsPlayer>();
        if (enemy != null)
        {
            enemy.recieveDmg(collisionDmg, collision.contacts[0].point);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            shieldRegeneration();
            if (health <= 0) destroyShip();
        }
    }
}