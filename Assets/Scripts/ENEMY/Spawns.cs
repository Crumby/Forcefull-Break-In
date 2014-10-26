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
            var vec = new Vector3(GameData.ActiveTrack.transform.position.x - GameData.ActiveTrack.renderer.bounds.size.x,
                GameData.ActiveTrack.transform.position.y,
                GameData.ActiveTrack.transform.position.z + zOffset);
            Instantiate(trackSpawn, vec, Quaternion.identity);
            leftPlatform = true;
        }
    }
    public void GenerateRightPlatform(float zOffset)
    {
        if (!rightPlatform)
        {
            var vec = new Vector3(GameData.ActiveTrack.transform.position.x + GameData.ActiveTrack.renderer.bounds.size.x,
                GameData.ActiveTrack.transform.position.y,
                GameData.ActiveTrack.transform.position.z + zOffset);
            Instantiate(trackSpawn, vec, Quaternion.identity);
            rightPlatform = true;
        }
    }

    private void SpawnOnAllTracks(GameObject toSpawn)
    {
        foreach (var track in GameData.Tracks)
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
            if (Player != null && !GameData.PauseGame)
            {
                if (Random.Range(0, 10) == 4 && !generatedPlatform)
                {
                    Debug.Log("Platform");
                    float signum = 1;
                    if (Random.Range(1, 25437) % 2 == 1)
                        signum = -1;
                    var vec = new Vector3(GameData.ActiveTrack.transform.position.x + signum * GameData.ActiveTrack.renderer.bounds.size.x,
                GameData.ActiveTrack.transform.position.y,
                GameData.ActiveTrack.transform.position.z + 4000);
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
            if (Player != null && !GameData.PauseGame)
            {
                for (int i = 0; i <= Random.Range(2, 50); i++)
                {
                    float hlp = Random.Range(0.0f, 300f);
                    if (Random.Range(0, 555) % 2 == 0)
                        hlp = -hlp;
                    var vec = new Vector3(spwanStart.x + GameData.ActiveTrack.transform.position.x + hlp,
                            spwanStart.y,
                            GameData.ActiveTrack.transform.position.z + spwanStart.z);
                    var obj = (GameObject)Instantiate(spawnObjects[Random.Range(0, 15642) % spawnObjects.Length],
                        vec,
                        Quaternion.identity);
                    obj.transform.parent = GameData.ActiveTrack.transform;
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
