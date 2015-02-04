using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    public enum EnemyCollision { RIGTH, LEFT, FRONT, BACK, UP, DOWN, NONE }
    public enum Difficulty { EASY = 1, NORMAL = 2, HARD = 4, GODLIKE = 6 }
    public enum ShopItems { HP, SHIELD, SHREGEN, MISLISEDMG, CANNONDMG, CANONFIRA, SPEED, ULTI }
    [System.Serializable]
    public class SaveData
    {
        public Difficulty difficulty;
        public float score, volume;
        public bool[][] planets;
        //public bool soundMute;
        public float bonusShields, bonusShieldRegen, bonusHP, bonusDmgMissise, bonusDmgCannon, firespeedCannon, bonusSpeed, ultiDerease;
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
    public static float totalScore { get; set; }
    public static motionPlayer playerMotion { get; private set; }
    public static shipSystemsPlayer playerSystems { get; private set; }
    public static gameData nonStatic { get; private set; }
    public string bonusPopup { get { return menus.getBonus(); } set { menus.setBonus(value); } }
    [Range(0, 5)]
    public float startDelay;
    public int gameEnd = 1;
    [Range(0, 2500)]
    public float endOff;
    public GameObject bounds;
    [Range(0.0F, 1000.0F)]
    public float aiActivationOffsite;
    public inGameMenu menus;
    public static Difficulty difficulty = Difficulty.EASY;
    public static int gameEnded { get; set; }
    //bonuses
    public static float bonusShields = 0, bonusShieldRegen = 0, bonusHP = 0, bonusDmgMissise = 0, bonusDmgCannon = 0, firespeedCannon = 1, bonusSpeed = 0, ultiDerease = 1;


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
        menus.showMenu();
        gameData.PauseGame();
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
        if (!gameData.pausedGame)
        {
            if (gameData.gameEnded == gameEnd)
            {
                totalScore += score;
                string n = Application.loadedLevelName;
                if (n == "space_0" || n == "space_1" || n == "space_2" || n == "space_3")
                    MenusLogic.stageCompleted = true;
                Debug.LogError(n+" "+gameData.totalScore);
                menus.showClearedStage();
                StartCoroutine(endRound());
                StartCoroutine(endRound());
                gameEnd++;
            }
        }
    }

    private IEnumerator endRound()
    {
        yield return new WaitForSeconds(5);
        LoadMenu();
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
        obj.score = totalScore;
        obj.planets = MenusLogic.levelsLocks;
        obj.bonusShields = bonusShields;
        obj.bonusShieldRegen = bonusShieldRegen;
        obj.bonusHP = bonusHP;
        obj.bonusDmgMissise = bonusDmgMissise;
        obj.bonusDmgCannon = bonusDmgCannon;
        obj.firespeedCannon = firespeedCannon;
        obj.bonusSpeed = bonusSpeed;
        obj.ultiDerease = ultiDerease;
        Save(obj);
    }

    public static void PeriodicSave()
    {
        var obj = new UnityEngine.SaveData();
        obj.difficulty = difficulty;
        obj.volume = AudioListener.volume;
        obj.score = totalScore;
        obj.planets = MenusLogic.levelsLocks;
        obj.bonusShields = bonusShields;
        obj.bonusShieldRegen = bonusShieldRegen;
        obj.bonusHP = bonusHP;
        obj.bonusDmgMissise = bonusDmgMissise;
        obj.bonusDmgCannon = bonusDmgCannon;
        obj.firespeedCannon = firespeedCannon;
        obj.bonusSpeed = bonusSpeed;
        obj.ultiDerease = ultiDerease;
        Save(obj);
    }

    public static void NewSave()
    {
        var obj = new UnityEngine.SaveData();
        obj.difficulty = difficulty;
        obj.volume = AudioListener.volume;
        obj.score = 0;
        obj.planets = MenusLogic.levelsLocks;
        obj.bonusShields = bonusShields;
        obj.bonusShieldRegen = bonusShieldRegen;
        obj.bonusHP = bonusHP;
        obj.bonusDmgMissise = bonusDmgMissise;
        obj.bonusDmgCannon = bonusDmgCannon;
        obj.firespeedCannon = firespeedCannon;
        obj.bonusSpeed = bonusSpeed;
        obj.ultiDerease = ultiDerease;
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
        MenusLogic.levelsLocks = obj.planets;
        bonusShields = obj.bonusShields;
        bonusShieldRegen = obj.bonusShieldRegen;
        bonusHP = obj.bonusHP;
        bonusDmgMissise = obj.bonusDmgMissise;
        bonusDmgCannon = obj.bonusDmgCannon;
        firespeedCannon = obj.firespeedCannon;
        bonusSpeed = obj.bonusSpeed;
        ultiDerease = obj.ultiDerease;
        return true;
    }

    public static void Upgrade(ShopItems item)
    {
        if (canUpgrade(item))
            switch (item)
            {
                case ShopItems.HP:
                    bonusHP += 50;
                    totalScore -= 1000;
                    break;
                case ShopItems.SHIELD:
                    bonusShields += 50;
                    totalScore -= 1000;
                    break;
                case ShopItems.SHREGEN:
                    totalScore -= 1000;
                    bonusShieldRegen += 2;
                    break;
                case ShopItems.MISLISEDMG:
                    totalScore -= 1000;
                    bonusDmgMissise += 20;
                    break;
                case ShopItems.CANNONDMG:
                    totalScore -= 1000;
                    bonusDmgCannon += 3;
                    break;
                case ShopItems.CANONFIRA:
                    totalScore -= 1000;
                    firespeedCannon += 0.25f;
                    break;
                case ShopItems.SPEED:
                    totalScore -= 1000;
                    bonusSpeed += 35;
                    break;
                case ShopItems.ULTI:
                    totalScore -= 1000;
                    ultiDerease -= 0.20f;
                    break;
            }
    }

    public static bool canUpgrade(ShopItems item)
    {
        switch (item)
        {
            case ShopItems.HP:
                return bonusHP == 0 && totalScore > 1000;
            case ShopItems.SHIELD:
                return bonusShields == 0 && totalScore > 1000;
            case ShopItems.SHREGEN:
                return bonusShieldRegen == 0 && totalScore > 1000;
            case ShopItems.MISLISEDMG:
                return bonusDmgMissise == 0 && totalScore > 1000;
            case ShopItems.CANNONDMG:
                return bonusDmgCannon == 0 && totalScore > 1000;
            case ShopItems.CANONFIRA:
                return firespeedCannon == 1 && totalScore > 1000;
            case ShopItems.SPEED:
                return bonusSpeed == 0 && totalScore > 1000;
            case ShopItems.ULTI:
                return ultiDerease == 1 && totalScore > 1000;
            default:
                return false;
        }
    }
}
