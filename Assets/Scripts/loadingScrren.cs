using UnityEngine;
using System.Collections;

public class loadingScrren : MonoBehaviour
{

    public GameObject[] dost;
    private int point = 1;
    // Use this for initialization
    public void MoveDot()
    {
        if (point >= dost.Length)
        {
            point = 1;
            for (int i = 1; i < dost.Length; i++)
                dost[i].SetActive(false);
        }
        else
        {
            dost[point].SetActive(true);
            point++;
        }
    }

}
