using UnityEngine;
using System.Collections;

public class Spawns : MonoBehaviour
{
    public static PlayerMotion Player;
    public Vector3 spwanStart;
    public GameObject[] spawnObjects;
    public bool endGame = false;
    public GameObject trackSpawn;
    //obsolete
    public static bool generatedPlatform = false;

    public static bool leftPlatform = false, rightPlatform = false;

    // Use this for initialization
    void Start()
    {
        if (spawnObjects.Length > 0)
            StartCoroutine(Mobs(1, 5));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextWave()
    {
        throw new System.Exception("Not implemented");
    }
    public void GenerateLeftPlatform(float zOffset)
    {
        if (!leftPlatform)
        {
            var vec = new Vector3(Player.activeTrack.transform.position.x - Player.activeTrack.renderer.bounds.size.x,
                Player.activeTrack.transform.position.y,
                Player.activeTrack.transform.position.z + zOffset);
            Instantiate(trackSpawn, vec, Quaternion.identity);
            leftPlatform = true;
        }
    }
    public void GenerateRightPlatform(float zOffset)
    {
        if (!rightPlatform)
        {
            var vec = new Vector3(Player.activeTrack.transform.position.x + Player.activeTrack.renderer.bounds.size.x,
                Player.activeTrack.transform.position.y,
                Player.activeTrack.transform.position.z + zOffset);
            Instantiate(trackSpawn, vec, Quaternion.identity);
            rightPlatform = true;
        }
    }

    private void SpawnOnAllTracks(GameObject toSpawn)
    {
        foreach (var track in PlayerMotion.Tracks)
            SpawnOnTrack(track, toSpawn);
    }
    //check needed
    // maybe redoo do 2D/3D
    private void SpawnOnTrack(GameObject track, GameObject toSpawn)
    {
        float xCoordiante = Random.Range(track.renderer.bounds.min.x,
            track.renderer.bounds.max.x);
        Instantiate(toSpawn, new Vector3(xCoordiante, spwanStart.y, spwanStart.z),
                        Quaternion.identity);
    }

    [System.Obsolete("Only for preview use")]
    IEnumerator Platform(int wait, int waitW)
    {
        while (!endGame)
        {
            if (Player != null && !PlayerMotion.Pause)
            {
                if (Random.Range(0, 10) == 4 && !generatedPlatform)
                {
                    Debug.Log("Platform");
                    float signum = 1;
                    if (Random.Range(1, 25437) % 2 == 1)
                        signum = -1;
                    var vec = new Vector3(Player.activeTrack.transform.position.x + signum * Player.activeTrack.renderer.bounds.size.x,
                Player.activeTrack.transform.position.y,
                Player.activeTrack.transform.position.z + 4000);
                    Instantiate(trackSpawn, vec, Quaternion.identity);
                    generatedPlatform = true;
                    yield return new WaitForSeconds(wait);
                }
                else
                    yield return new WaitForSeconds(wait);
            }
            else
            {
                //wait for init all components
                yield return new WaitForSeconds(waitW);
            }
        }
    }

    [System.Obsolete("Only for preview use")]
    IEnumerator Mobs(int wait, int waitW)
    {
        while (!endGame)
        {
            if (Player != null && !PlayerMotion.Pause)
            {
                for (int i = 0; i <= Random.Range(2, 50); i++)
                {
                    float hlp = Random.Range(0.0f, 300f);
                    float hlpp = 80;
                    if (Random.Range(0, 555) % 2 == 0)
                        hlp = -hlp;
                    if (Random.Range(0, 555) % 2 == 0)
                        hlpp = 0;
                    var vec = new Vector3(spwanStart.x + Player.activeTrack.transform.position.x + hlp,
                            spwanStart.y + hlpp,
                            Player.activeTrack.transform.position.z + spwanStart.z);
                    Instantiate(spawnObjects[Random.Range(0, 15642) % spawnObjects.Length],
                        vec,
                        Quaternion.identity);
                    yield return new WaitForSeconds(wait);
                }
                yield return new WaitForSeconds(waitW);
            }
            else
            {
                //wait for init all components
                yield return new WaitForSeconds(waitW);
            }
        }
    }
}
