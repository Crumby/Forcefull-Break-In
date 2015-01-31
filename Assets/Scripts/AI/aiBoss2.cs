using UnityEngine;
using System.Collections;

public class aiBoss2 : MonoBehaviour
{

    public basicEnemySystems[] subparts;
    public GameObject endExplosion;
    public Transform[] explosionPositions;
    private Animator anim;
    [HideInInspector]
    public bool animEnded = false;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && gameData.inReach(transform.position))
        {
            if (!animEnded)
            {
                bool end = true;
                foreach (var i in subparts)
                    if (i.health > 0)
                    {
                        end = false;
                        break;
                    }
                if (end)
                {
                    foreach (var pos in explosionPositions)
                    {
                        var exp = (GameObject)Instantiate(endExplosion, pos.position, Quaternion.identity);
                        exp.transform.parent = transform;
                    }
                    anim.Play("lvl2Boss");
                }
            }
            else gameData.gameEnded++;

        }
    }
}
