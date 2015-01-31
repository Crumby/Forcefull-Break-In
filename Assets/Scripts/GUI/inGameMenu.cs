using UnityEngine;
using System.Collections;

public class inGameMenu : MonoBehaviour
{

    public UnityEngine.UI.Button menu, restart, exit, options;
    public UnityEngine.UI.Text gameOver,stageCleared, bonusPopup;
    public UnityEngine.UI.Scrollbar volume;
    public UnityEngine.UI.Toggle sound;
    public GameObject optionsMenu;

    void Start()
    {
        Screen.showCursor = false;
        volume.value = AudioListener.volume;
        if (volume.value == 0)
        {
            volume.interactable = false;
            sound.isOn = false;
        }
    }

    public void reset()
    {
        Screen.showCursor = false;
        menu.enabled = false;
        restart.enabled = false;
        exit.enabled = false;
        gameOver.enabled = false;
        options.enabled = false;
        optionsMenu.SetActive(false);
    }

    public void showClearedStage()
    {
        stageCleared.gameObject.SetActive(true);
        float timer = 0;
        while (timer <= 6000)
            timer += Time.fixedDeltaTime;
    }

    public void showMenu()
    {
        Screen.showCursor = !Screen.showCursor;
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        restart.gameObject.SetActive(!restart.gameObject.activeSelf);
        exit.gameObject.SetActive(!exit.gameObject.activeSelf);
        options.gameObject.SetActive(!options.gameObject.activeSelf);
        optionsMenu.SetActive(false);
    }

    public void showGameOver()
    {
        Screen.showCursor = true;
        bonusPopup.enabled = false;
        menu.gameObject.SetActive(false);
        options.gameObject.SetActive(false);
        exit.transform.position = options.transform.position;
        gameOver.gameObject.SetActive(!gameOver.gameObject.activeSelf);
    }

    public void setBonus(string str)
    {
        bonusPopup.text = str;
    }
    public string getBonus()
    {
        return bonusPopup.text;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && Time.timeScale == 1)
        {
            showMenu();
            gameData.PauseGame();
        }
    }
    public void SoundVolume(float val)
    {
        if (val < 0.1f)
            AudioListener.volume = 0.1f;
        else
            AudioListener.volume = val;
    }

    public void SoundEnable(bool val)
    {
        if (val)
            AudioListener.volume = volume.value;
        else
            AudioListener.volume = 0;
    }
}
