using UnityEngine;
using System.Collections;

public class PlatformMotion : MonoBehaviour
{

    public static PlayerMotion Player;
    private float speed = 2500;
    private float z = 541;

    // Use this for initialization
    void Start()
    {
        PlayerMotion.Tracks.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMotion.Pause)
        {

            if (Player.activeTrack != this.gameObject)
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            else if (Player.activeTrack.transform.position.z > z)
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            if (renderer.bounds.max.z < z)
            {
                PlayerMotion.Tracks.Remove(gameObject);
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
