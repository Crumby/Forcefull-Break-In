﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MenuScreen { NONE, WELCOME, LEVEL }
namespace UnityEngine
{
    public enum PlanetNames { Garuz, Figil, Prezz, Bcolg }
}
public class MenusLogic : MonoBehaviour
{
    public PlanetEntity initValue;
    public Scrollbar volume;
    public Text difficulutyText, totalScore;
    public Toggle sound;
    public GameObject welcomeSC, levelSC, panel;
    public Text planetName, stageT, start;
    public Button stage, contin;
    public PlanetEntity[] planets;
    public static bool[][] levelsLocks;
    private PlanetEntity selectedPlanet;
    private static PlanetNames SelectedPlanet;
    private static MenuScreen loadedScreen = MenuScreen.NONE;
    public static ShopItems SelectedBonus;
    public static bool stageCompleted = false;
    public loadingScrren loadingScreen;
    public Button shOff, shOn;
    public GameObject shPanel, levelCh;
    // Use this for initialization
    void Start()
    {
        if (MenuScreen.NONE == loadedScreen)
            initGame();
        if (loadedScreen == MenuScreen.LEVEL)
        {
            welcomeSC.SetActive(false);
            levelSC.SetActive(true);
            LevelPanelMove(planets[(int)SelectedPlanet]);
            if (stageCompleted)
            {
                planets[(int)SelectedPlanet].unlockNext();
                stageCompleted = false;
                gameData.EndRoundSave();
            }
        }
        else
        {
            welcomeSC.SetActive(true);
            levelSC.SetActive(false);
            loadedScreen = MenuScreen.WELCOME;
            LevelPanelMove(initValue);
        }
        ReloadTotalScore();
    }

    public void setNewGame()
    {
        gameData.bonusShields = 0;
        gameData.bonusShieldRegen = 0;
        gameData.bonusHP = 0;
        gameData.bonusDmgMissise = 0;
        gameData.bonusDmgCannon = 0;
        gameData.firespeedCannon = 1;
        gameData.bonusSpeed = 0;
        gameData.ultiDerease = 1;
        gameData.totalScore = 0;
        levelsLocks = new bool[planets.Length][];
        for (int i = 0; i < planets.Length; i++)
        {
            levelsLocks[i] = new bool[planets[i].LevelsLock.Length];
            for (int l = 0; l < planets[i].LevelsLock.Length; l++)
            {
                levelsLocks[i][l] = planets[i].LevelsLock[l];
            }
        }
        selectedPlanet.SelectedLevel = 1;
        ReloadTotalScore();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            switch (loadedScreen)
            {
                case MenuScreen.WELCOME:
                    PeriodicSave();
                    QuitGame();
                    break;
                case MenuScreen.LEVEL:
                    if (shPanel.activeSelf)
                    {
                        shPanel.SetActive(false);
                        levelCh.SetActive(true);
                        shOff.gameObject.SetActive(true);
                        shOn.gameObject.SetActive(false);
                    }
                    else
                    {
                        PeriodicSave();
                        levelSC.SetActive(false);
                        welcomeSC.SetActive(true);
                        loadedScreen = MenuScreen.WELCOME;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void initGame()
    {
        if (!gameData.LoadData())
        {
            gameData.difficulty = Difficulty.EASY;
            contin.interactable = false;
            if (levelsLocks == null)
            {
                levelsLocks = new bool[planets.Length][];
                for (int i = 0; i < planets.Length; i++)
                {
                    levelsLocks[i] = new bool[planets[i].LevelsLock.Length];
                    for (int l = 0; l < planets[i].LevelsLock.Length; l++)
                    {
                        levelsLocks[i][l] = planets[i].LevelsLock[l];
                    }
                }
            }
        }
        else
        {
            contin.interactable = true;
        }
        volume.value = AudioListener.volume;
        if (AudioListener.volume == 0)
        {
            volume.interactable = false;
            sound.isOn = false;
        }
        difficulutyText.text = gameData.difficulty.ToString();
    }

    public void ChangeStage()
    {
        selectedPlanet.NextLevel();
        updateStage();
    }

    private void updateStage()
    {
        stageT.text = selectedPlanet.SelectedLevel.ToString();
        start.enabled = selectedPlanet.IsLocked();
    }

    public void LevelPanelMove(PlanetEntity o)
    {
        selectedPlanet = o;
        if (panel != null)
            panel.transform.position = new Vector3(o.transform.position.x, panel.transform.position.y, panel.transform.position.z);
        if (stage != null)
            stage.transform.position = new Vector3(o.transform.position.x, stage.transform.position.y, stage.transform.position.z);
        if (planetName != null)
            planetName.text = selectedPlanet.PlanetName.ToString();
        SelectedPlanet = selectedPlanet.PlanetName;
        updateStage();
    }

    public void LoadLevel()
    {
        switch (selectedPlanet.PlanetName)
        {
            case PlanetNames.Garuz:
                switch (selectedPlanet.SelectedLevel)
                {
                    case 1: StartCoroutine(LoadLevelAsync("space_0"));
                        break;
                    case 2: StartCoroutine(LoadLevelAsync("lvl2"));
                        break;
                }
                break;
            case PlanetNames.Figil:
                switch (selectedPlanet.SelectedLevel)
                {
                    case 1: StartCoroutine(LoadLevelAsync("space_1"));
                        break;
                    case 2: StartCoroutine(LoadLevelAsync("lvl3"));
                        break;
                }
                break;
            case PlanetNames.Prezz:
                switch (selectedPlanet.SelectedLevel)
                {
                    case 1: StartCoroutine(LoadLevelAsync("space_2"));
                        break;
                    case 2: StartCoroutine(LoadLevelAsync("lvl4"));
                        break;
                }
                break;
            case PlanetNames.Bcolg:
                switch (selectedPlanet.SelectedLevel)
                {
                    case 1: StartCoroutine(LoadLevelAsync("space_3"));
                        break;
                    case 2: StartCoroutine(LoadLevelAsync("lvl5"));
                        break;
                }
                break;
            default:
                break;
        }
    }

    IEnumerator LoadLevelAsync(string name)
    {
        loadingScreen.gameObject.SetActive(true);
        AsyncOperation sceneLoadingOperation = Application.LoadLevelAsync(name);
        while (!sceneLoadingOperation.isDone)
        {
            loadingScreen.MoveDot();
            yield return new WaitForSeconds(0.2f);
        }
        loadingScreen.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScreen()
    {
        if (MenusLogic.loadedScreen == MenuScreen.WELCOME)
        {
            MenusLogic.loadedScreen = MenuScreen.LEVEL;
            contin.interactable = true;
        }
        else if (MenusLogic.loadedScreen == MenuScreen.LEVEL)
            MenusLogic.loadedScreen = MenuScreen.WELCOME;
    }

    public void SetDifficulty()
    {
        switch (gameData.difficulty)
        {
            case Difficulty.EASY:
                gameData.difficulty = Difficulty.NORMAL;
                break;
            case Difficulty.NORMAL:
                gameData.difficulty = Difficulty.HARD;
                break;
            case Difficulty.HARD:
                gameData.difficulty = Difficulty.GODLIKE;
                break;
            case Difficulty.GODLIKE:
                gameData.difficulty = Difficulty.EASY;
                break;
        }
        difficulutyText.text = gameData.difficulty.ToString();
    }

    public void SoundVolume(float val)
    {
        if (val < 0.1f)
            AudioListener.volume = 0.1f;
        else
            AudioListener.volume = val;
    }

    public void PeriodicSave()
    {
        gameData.PeriodicSave();
    }

    public void NewGameSave()
    {
        gameData.NewSave();
    }

    public void SoundEnable(bool val)
    {
        if (val)
            AudioListener.volume = volume.value;
        else
            AudioListener.volume = 0;
    }

    public void ReloadTotalScore()
    {
        totalScore.text = gameData.totalScore.ToString();
    }

    public void buyBonus()
    {
        gameData.Upgrade(SelectedBonus);
        ReloadTotalScore();
    }
}
