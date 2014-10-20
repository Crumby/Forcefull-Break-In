using UnityEngine;
using System.Collections;

public class SpinerAI : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMotion.Pause)
        {
            transform.Rotate(0, 5, 0, Space.Self);
        }
    }
}
