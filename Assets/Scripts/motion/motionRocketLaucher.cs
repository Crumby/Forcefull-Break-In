using UnityEngine;
using System.Collections;

public class motionRocketLaucher : MonoBehaviour
{

    public GameObject engine;
    [Range(0, 2F)]
    public float timeDown, timeBackward;
    public Transform navigation;
    private bool init = false;

    // Use this for initialization
    void Start()
    {
        engine.SetActive(false);
        navigation = null;
    }

    public void Fire()
    {
        engine.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (navigation != null) {
                transform.LookAt(navigation.position);
            }
            if (!init && engine.activeSelf)
            {
                if (timeDown > 0)
                {
                    transform.Translate(Vector3.down * Time.deltaTime);
                    timeDown -= Time.deltaTime;
                }
                if (timeBackward > 0)
                {
                    transform.Translate(Vector3.back * Time.deltaTime);
                    timeBackward -= Time.deltaTime;
                }
                if (timeDown <= 0 && timeBackward <= 0)
                {
                    init = (GetComponent<motionProjectile>().launch = true);
                    transform.parent = null;
                }
            }
        }
    }
}
