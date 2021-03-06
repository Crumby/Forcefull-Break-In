﻿using UnityEngine;
using System.Collections;

public class TerrainCollision : MonoBehaviour
{

    public GameObject explosion;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            Physics.IgnoreCollision(other.collider, collider);
        }
        Instantiate(explosion, transform.position, transform.rotation);
        GameObject.Destroy(gameObject);
    }
}
