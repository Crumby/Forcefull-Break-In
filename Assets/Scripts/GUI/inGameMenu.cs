using UnityEngine;
using System.Collections;

public class inGameMenu : MonoBehaviour
{

    public UnityEngine.UI.Button menu, restart, exit;
    public UnityEngine.UI.Text gameOver;

    public void reset()
    {
        Screen.showCursor = false;
        menu.enabled = false;
        restart.enabled = false;
        exit.enabled = false;
        gameOver.enabled = false;
    }

    public void showMenu()
    {
        Screen.showCursor = !Screen.showCursor;
        menu.gameObject.SetActive(!menu.gameObject.activeSelf);
        restart.gameObject.SetActive(!restart.gameObject.activeSelf);
        exit.gameObject.SetActive(!exit.gameObject.activeSelf);
    }

    public void showGameOver()
    {
        menu.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(!gameOver.gameObject.activeSelf);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")&&Time.timeScale==1)
        {
            showMenu();
            gameData.PauseGame();
        }
    }
}
