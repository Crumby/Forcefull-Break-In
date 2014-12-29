using UnityEngine;
using System.Collections;

public class weaponRocketLaucher : MonoBehaviour
{
    public GameObject projectilePrefab;
    [Range(0.0F, 5.0F)]
    public float reloadTime;
    public bool charged { get { if (projetile == null)return false; else return true; } }
    private GameObject projetile;
    private float timer;
    public bool roateAround { get; set; }
    // Use this for initialization
    void Start()
    {
        roateAround = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (GetComponentInParent<motionPlayer>() != null)
                //                Debug.Log("P" + charged + " " + timer + " " + projetile);
                if (charged && projetile.GetComponent<motionProjectile>().launch)
                {
                    projetile = null;
                }
            if (!charged)
                timer += Time.deltaTime;
            if (timer >= reloadTime && !charged)
            {
                projetile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projetile.transform.localRotation = transform.rotation;
                projetile.transform.parent = transform;
                timer = 0;
            }
        }
    }

    private void addParentSpeed(bool isPlayer)
    {
        if (isPlayer)
            projetile.GetComponent<motionProjectile>().forwardSpeed += GetComponentInParent<motionPlayer>().forwardSpeed;
        else
            projetile.GetComponent<motionProjectile>().forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
    }

    public void Fire(Transform navigation, Vector3 to, bool isPlayer)
    {
        if (charged)
        {
            if (navigation != null)
                projetile.GetComponent<animRocketLaucher>().navigation = navigation;
            if (isPlayer)
                projetile.transform.LookAt(gameData.aimPoint, Vector3.up);
            addParentSpeed(isPlayer);
            projetile.GetComponent<motionProjectile>().directionVector = to;
            projetile.GetComponent<motionProjectile>().isPlayers = isPlayer;
            if (projetile.GetComponent<animRocketLaucher>() != null)
                projetile.GetComponent<animRocketLaucher>().Fire();
            else
            {
                if (roateAround)
                    projetile.transform.rotation = Quaternion.Euler(0, 180, 0);
                projetile.GetComponent<motionProjectile>().launch = true;
                projetile.transform.parent = null;
            }

        }
    }
}
