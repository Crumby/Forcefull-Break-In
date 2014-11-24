using UnityEngine;
using System.Collections;

public class aimingControler : MonoBehaviour
{

    public UnityEngine.UI.RawImage pointToCover;

    void Start()
    {
        Screen.showCursor = false;
    }

    private void targetFinder()
    {
        RaycastHit contact;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(transform.position), out contact, gameData.aiActivation) && contact.transform.gameObject.GetComponent<motionEnemy>() != null)
        {
            GetComponent<UnityEngine.UI.RawImage>().color = Color.red;
            gameData.aimPoint = contact.transform.position;
            gameData.aimNavigation = contact.transform;
            pointToCover.gameObject.SetActive(false);
        }
        else if (Physics.Raycast(gameData.playerPosition, Vector3.forward, out contact, gameData.aiActivation) && contact.transform.gameObject.GetComponent<terainDestroy>() == null && contact.transform.gameObject.GetComponent<motionProjectile>() == null)
        {
            pointToCover.gameObject.SetActive(true);
            gameData.aimPoint = new Vector3(gameData.playerPosition.x, gameData.playerPosition.y, contact.transform.position.z);
            gameData.aimNavigation = null;
            GetComponent<UnityEngine.UI.RawImage>().color = Color.white;
        }
        else
        {
            pointToCover.gameObject.SetActive(true);
            GetComponent<UnityEngine.UI.RawImage>().color = Color.white;
            gameData.aimPoint = new Vector3(0, 0, gameData.aiActivation) + gameData.playerPosition;
            gameData.aimNavigation = null;
        }
        pointToCover.transform.position = Camera.main.WorldToScreenPoint(gameData.aimPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame && Input.mousePresent)
            targetFinder();
    }

}
