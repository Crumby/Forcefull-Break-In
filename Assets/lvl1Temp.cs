using UnityEngine;
using System.Collections;

public class lvl1Temp : MonoBehaviour
{

    public UnityEngine.UI.Button menu, restart, exit;
    public UnityEngine.UI.Text gameOver;

    public void end()
    {
        menu.enabled = false;
        restart.enabled = true;
        exit.enabled = true;
        gameOver.enabled = true;
    }

    public void endG()
    {
        menu.enabled = false;
        restart.enabled = true;
        exit.enabled = true;
    }

}
