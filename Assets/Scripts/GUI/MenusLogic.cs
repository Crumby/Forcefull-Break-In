using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenusLogic : MonoBehaviour
{
    public PlanetEntity initValue;
    public UnityEngine.UI.Scrollbar volume;
    public UnityEngine.UI.Text difficulutyText;
    public UnityEngine.UI.Toggle sound;
    public static PlanetEntity SelectedPlanet { get; private set; }
    private static Component panel;
    private static Button stage;
    private static Text planetName, stageT, start;
    // Use this for initialization
    void Start()
    {
        initGame();
        var hlp = GameObject.Find("LevelScreen");
        hlp.SetActive(true);
        panel = GetObject<Component>("LV_Panel");
        planetName = GetObject<Text>("S_Name");
        stageT = GetObject<Text>("ST_Text");
        stage = GetObject<Button>("ST_Button");
        start = GetObject<Text>("S_Text");
        hlp.SetActive(false);
        LevelPanelMove(initValue);
        initValue = null;
    }

    private void initGame()
    {
        if (!gameData.LoadData())
            gameData.difficulty = Difficulty.EASY;
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
        SelectedPlanet.NextLevel();
        updateStage();
    }

    private void updateStage()
    {
        stageT.text = SelectedPlanet.SelectedLevel.ToString();
        start.enabled = SelectedPlanet.IsLocked();
    }

    public void LevelPanelMove(PlanetEntity o)
    {
        MenusLogic.SelectedPlanet = o;
        if (panel != null)
            panel.transform.position = new Vector3(o.transform.position.x, panel.transform.position.y, panel.transform.position.z);
        if (stage != null)
            stage.transform.position = new Vector3(o.transform.position.x, stage.transform.position.y, stage.transform.position.z);
        if (planetName != null)
            planetName.text = SelectedPlanet.PlanetName;
        updateStage();
    }

    public static T GetObject<T>(string name) where T : UnityEngine.Component
    {
        var obj = GameObject.Find(name);
        if (obj != null)
        {
            var sobj = obj.GetComponent<T>();
            if (sobj != null)
                return sobj;
        }
        return null;
    }

    public void LoadLevel()
    {
        switch (planetName.text)
        {
            case ("Karin"):
                Application.LoadLevel("lvl1");
                break;
            case ("Anet"):
                Application.LoadLevel("lvl2");
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
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

    public void SoundEnable(bool val)
    {
        if (val)
            AudioListener.volume = volume.value;
        else
            AudioListener.volume = 0;
    }
}
