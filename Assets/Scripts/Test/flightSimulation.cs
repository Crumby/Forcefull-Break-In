using UnityEngine;
using System.Collections;

public class flightSimulation : MonoBehaviour
{


    public int speed = 100;

    // Update is called once per frame
    void Update()
    {
        if (!mov.Pause)
        {
            if (transform.position.z < -4500)
                transform.position = new Vector3(transform.position.x,
                    transform.position.y, 7361.5f);
            else
                transform.Translate(Vector3.back * speed);
        }
    }
}
