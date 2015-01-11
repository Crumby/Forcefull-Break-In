using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    public enum EnemyCollision { RIGTH, LEFT, FRONT, BACK, UP, DOWN, NONE }
    public enum Difficulty { EASY = 1, NORMAL = 2, HARD = 4, GODLIKE = 6 }
    [System.Serializable]
    public class SaveData
    {
        public Difficulty difficulty;
        public float score, volume;
        //public bool soundMute;
    }
}

public class gameData : MonoBehaviour
{
    public const string dataFile = "./FBI.save";
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
    public static int score
    {
        get { if (playerSystems != null)return playerSystems.Score; return 0; }
        set { if (playerSystems != null)playerSystems.Score += value * ScoreMultiplier; }
    }
    public static int ScoreMultiplier = 1;
    public static int addPower
    {
        set { if (playerSystems != null)playerSystems.Power += value; }
    }
    public static Vector3 aimPoint { get; set; }
    public static Transform aimNavigation { get; set; }
    public static float endOffsite { get; private set; }
    public static float totalScore { get; private set; }
    public static motionPlayer playerMotion { get; private set; }
    public static shipSystemsPlayer playerSystems { get; private set; }
    public static gameData nonStatic { get; private set; }
    public string bonusPopup { get { return menus.getBonus(); } set { menus.setBonus(value); } }
    [Range(0, 5)]
    public float startDelay;
    public int gameEnd = 1;
    [Range(0, 500)]
    public float endOff;
    public GameObject bounds;
    [Range(0.0F, 1000.0F)]
    public float aiActivationOffsite;
    public inGameMenu menus;
    public static Difficulty difficulty { get; set; }
    public static int gameEnded { get; set; }


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
        if (difficulty == 0)
            difficulty = Difficulty.EASY;
        gameData.nonStatic = this;
        gameData.gameEnded = 0;
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
        pausedGame = false;
        ScoreMultiplier = 1;
        nonStatic = null;
        gameData.gameEnded = 0;
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
        if (!gameData.pausedGame) {
            if (gameData.gameEnded == gameEnd) {
                gameData.EndRoundSave();
                LoadMenu();
            }
        }
    }

    void LoadMenu()
    {
        reset();
        Screen.showCursor = true;
        Application.LoadLevel("welcomeMenu");
    }

    public static void EndRoundSave()
    {
        var obj = new UnityEngine.SaveData();
        obj.difficulty = difficulty;
        obj.volume = AudioListener.volume;
        obj.score = totalScore + score;
        Save(obj);
    }

    public static void PeriodicSave()
    {
        var obj = new UnityEngine.SaveData();
        obj.difficulty = difficulty;
        obj.volume = AudioListener.volume;
        obj.score = totalScore;
        Save(obj);
    }

    public static void NewSave()
    {
        var obj = new UnityEngine.SaveData();
        obj.difficulty = difficulty;
        obj.volume = AudioListener.volume;
        obj.score = 0;
        Save(obj);
    }

    private static void Save(UnityEngine.SaveData obj)
    {
        System.IO.Stream stream = System.IO.File.Open(gameData.dataFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
        System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        formatter.Serialize(stream, obj);
        stream.Close();
    }

    public static bool LoadData()
    {
        if (!System.IO.File.Exists(gameData.dataFile))
            return false;
        System.IO.Stream stream = System.IO.File.Open(gameData.dataFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
        if (!stream.CanRead)
            return false;
        System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        var obj = (UnityEngine.SaveData)formatter.Deserialize(stream);
        stream.Close();

        totalScore = obj.score;
        difficulty = obj.difficulty;
        AudioListener.volume = obj.volume;
        return true;
    }
}
