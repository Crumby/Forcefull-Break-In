using UnityEngine;
using System.Collections;

public class moveProjectile : MonoBehaviour
{
    public int timeToLive = 5;
    public int speed = 50;
    // Update is called once per frame

    void Awake()
    {
        //redo for pause
        if (timeToLive != 0)
            Destroy(this.gameObject, timeToLive);
    }

    void Update()
    {
        if (!mov.Pause)
        {
            transform.Translate(Vector3.forward * speed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Player")
        {
            mov.Score += collision.gameObject.GetComponent<MoverO>().Score;
            Destroy(collision.gameObject);
        }
    }
}
