using UnityEngine;
using System.Collections;

public class MoverO : MonoBehaviour
{
    public float speed;
    public int Score = 10;

    void Update()
    {
        if (!mov.Pause)
        {
            transform.Translate(Vector3.back * speed);
            if (transform.position.z < -100)
                Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
    }

}
