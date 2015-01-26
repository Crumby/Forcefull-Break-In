using UnityEngine;
using System.Collections;

public class aiBoss2 : MonoBehaviour
{

    public basicEnemySystems[] subparts;
       
    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            bool end = true;
            foreach(var i in subparts)
                if (i.health > 0)
                {
                    end = false;
                    break;
                }
            if (end)
                gameData.gameEnded++;
        }   
    }
}
