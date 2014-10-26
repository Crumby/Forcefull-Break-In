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
        GameData.Tracks.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameData.PauseGame)
        {

            if (GameData.ActiveTrack != this.gameObject)
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            else if (GameData.ActiveTrack.transform.position.z > z)
            {
                transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
            if (renderer.bounds.max.z < z)
            {
                GameData.Tracks.Remove(gameObject);
                Destroy(gameObject);
                if (GameData.ActiveTrack.transform.position.x < transform.position.x)
                    Spawns.rightPlatform = false;
                else
                    Spawns.leftPlatform = false;
                //obsolete
                Spawns.generatedPlatform = false;
            }
        }
    }
}
