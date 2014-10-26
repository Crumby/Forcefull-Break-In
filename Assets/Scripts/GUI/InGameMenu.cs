using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    public GameObject EndGameText;

    //redoo
    public void GameOver()
    {
        GameData.PauseGame = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        GameData.ResetData();
        EndGameText.SetActive(true);
        yield return new WaitForSeconds(5);
        MainMenu();
    }

    public void MainMenu()
    {
        Application.LoadLevel("welcomeMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
