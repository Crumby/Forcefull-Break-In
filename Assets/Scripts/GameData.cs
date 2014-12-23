using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    public enum EnemyCollision { RIGTH, LEFT, FRONT, BACK, UP, DOWN, NONE }
}

public class gameData : MonoBehaviour
{

    public static bool pausedGame { get; set; }
    public static GameObject gameBounds { get; private set; }
    public static Vector3 cameraOffsite { get; private set; }
    public static Vector3 playerPosition
    {
        get { if (playerMotion != null)return playerMotion.transform.position; else return Vector3.one; }
    }
    public static float forwardSpeed
    {
        get { if (playerMotion != null)return playerMotion.forwardSpeed; else return 0; }
    }
    public static float horizontalSpeed
    {
        get { if (playerMotion != null)return playerMotion.horizontalSpeed; else return 0; }
    }
    public static float verticalSpeed
    {
        get { if (playerMotion != null)return playerMotion.verticalSpeed; else return 0; }
    }
    public static float aiActivation { get; private set; }
    public static int addScore
    {
        set { if (playerSystems != null)playerSystems.Score += value * ScoreMultiplier; }
    }
    public static int ScoreMultiplier=1;
    public static int addPower
    {
        set { if (playerSystems != null)playerSystems.Power += value; }
    }
    public static Vector3 aimPoint { get; set; }
    public static Transform aimNavigation { get; set; }
    public static float endOffsite { get; private set; }
    public static motionPlayer playerMotion { get; private set; }
    public static shipSystemsPlayer playerSystems { get; private set; }
    [Range(0, 5)]
    public float startDelay;
    [Range(0, 500)]
    public float endOff;
    public GameObject bounds;
    [Range(0.0F, 1000.0F)]
    public float aiActivationOffsite;
    public inGameMenu menus;


    // Use this for initialization
    void Start()
    {
        if (startDelay != 0)
        {
            pausedGame = true;
            Time.timeScale = 0;
        }
        gameBounds = bounds;
        aiActivation = aiActivationOffsite;
        endOffsite = endOff;
    }

    public static bool isChildOfPlayer(Transform who)
    {
        if (playerMotion != null) return who.IsChildOf(playerMotion.transform);
        return false;
    }

    public static bool inReach(Vector3 position)
    {
        return gameData.aiActivation + gameData.playerPosition.z >= position.z;
    }

    public static void initPlayer(motionPlayer pl)
    {
        playerMotion = pl;
        playerSystems = pl.GetComponent<shipSystemsPlayer>();
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
        playerMotion = null;
        playerSystems = null;
        gameBounds = null;
        Time.timeScale = 1;
        cameraOffsite = Vector3.zero;
        aimNavigation = null;
        endOffsite = 0;
        menus.reset();
        pausedGame = false;
    }

    public void pauseGame()
    {
        gameData.PauseGame();
        menus.showMenu();
    }

    public static void PauseGame()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else Time.timeScale = 0;
        gameData.pausedGame = !pausedGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (startDelay > 0)
        {
            if (Input.anyKeyDown)
            {
                PauseGame();
                startDelay = 0;
            }
            else
            {
                startDelay -= 0.01f;
                if (startDelay < 0) PauseGame();
            }
        }
    }

    void LoadMenu()
    {
        Application.LoadLevel("welcomeMenu");
    }
}
