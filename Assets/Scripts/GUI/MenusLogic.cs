using UnityEngine;
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
    private PlanetEntity selectedPlanet;
    private static PlanetNames SelectedPlanet;
    private static MenuScreen loadedScreen = MenuScreen.NONE;
    // Use this for initialization
    void Start()
    {
        initGame();
        if (loadedScreen == MenuScreen.LEVEL)
        {
            welcomeSC.SetActive(false);
            levelSC.SetActive(true);
            LevelPanelMove(planets[(int)SelectedPlanet]);
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

    private void initGame()
    {
        if (!gameData.LoadData())
        {
            gameData.difficulty = Difficulty.EASY;
            contin.interactable = false;
        }
        else
            contin.interactable = true;
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
                Application.LoadLevel("lvl1");
                break;
            case PlanetNames.Figil:
                Application.LoadLevel("lvl2");
                break;
            case PlanetNames.Prezz:
                Application.LoadLevel("lvl1");
                break;
            case PlanetNames.Bcolg:
                Application.LoadLevel("lvl2");
                break;
            default:
                break;
        }
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
}
