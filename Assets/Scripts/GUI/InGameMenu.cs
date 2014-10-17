using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour
{

    public GameObject EndGameText;

    //redoo
    public void GameOver()
    {
        PlayerMotion.Pause = true;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
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
