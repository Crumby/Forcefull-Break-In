﻿using UnityEngine;
using System.Collections;

public class gameData : MonoBehaviour
{
    public static bool pausedGame { get; set; }
    public static GameObject gameBounds { get; private set; }
    public static Vector3 cameraOffsite { get; private set; }
    public static Vector3 playerPosition
    {
        get { if (player != null)return player.transform.position; else return Vector3.one; }
    }
    public static float forwardSpeed
    {
        get { if (player != null)return player.forwardSpeed; else return 0; }
    }
    public static float horizontalSpeed
    {
        get { if (player != null)return player.horizontalSpeed; else return 0; }
    }
    public static float verticalSpeed
    {
        get { if (player != null)return player.verticalSpeed; else return 0; }
    }
    public static float aiActivation { get; private set; }
    public static int addScore
    {
        set { if (player != null)player.GetComponent<shipSystemsPlayer>().Score += value; }
    }

    private static motionPlayer player;
    public GameObject bounds;
    [Range(0.0F, 1000.0F)]
    public float aiActivationOffsite;


    // Use this for initialization
    void Start()
    {
        pausedGame = false;
        gameBounds = bounds;
        aiActivation = aiActivationOffsite;
    }

    public static bool isChildOfPlayer(Transform who)
    {
        if (player != null) return who.IsChildOf(player.transform);
        return false;
    }

    public static bool inReach(Vector3 position)
    {
        return gameData.aiActivation + gameData.playerPosition.z >= position.z;
    }

    public static void initPlayer(motionPlayer pl)
    {
        player = pl;
        cameraOffsite -= playerPosition;
    }

    public static void initCamera(motionCamera camera)
    {
        cameraOffsite += camera.transform.position;
    }

    public void reloadLevel()
    {
        reset();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void reset()
    {
        player = null;
        gameBounds = null;
        Time.timeScale = 1;
        cameraOffsite = Vector3.zero;
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else Time.timeScale = 0;
        gameData.pausedGame = !pausedGame;
    }

    // Update is called once per frame
    void Update()
    {
        aiActivation = aiActivationOffsite;
    }
}