using UnityEngine;
using System.Collections;

public class moverPlatform : MonoBehaviour
{

    public static mov Player;
    private float speed = 25f;
    private float z = 541;

    // Use this for initialization
    void Start()
    {
        mov.tracks.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mov.Pause)
        {

            if (Player.activeTrack != this.gameObject)
            {
                transform.Translate(Vector3.back * speed);
            }
            else if (Player.activeTrack.transform.position.z > z)
            {
                transform.Translate(Vector3.back * speed);
            }
            if (renderer.bounds.max.z < z)
            {
                mov.tracks.Remove(gameObject);
                Destroy(gameObject);
                if (Player.activeTrack.transform.position.x < transform.position.x)
                    Spawns.rightPlatform = false;
                else
                    Spawns.leftPlatform = false;
                //obsolete
                Spawns.generatedPlatform = false;
            }
        }
    }
}
