using UnityEngine;
using System.Collections;

public class weaponRocketLaucher : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Vector3 destination { get; set; }
    [Range(0.0F, 5.0F)]
    public float reloadTime;
    public bool charged { get; private set; }
    private GameObject projetile;
    private float timer;
    // Use this for initialization
    void Start()
    {
        destination = Vector3.zero;
        charged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.pausedGame)
        {
            if (projetile != null && projetile.GetComponent<motionProjectile>().launch)
            {
                projetile = null;
                charged = false;
            }
            if (!charged)
                timer += Time.deltaTime;
            if (timer >= reloadTime && !charged)
            {
                projetile = (GameObject)Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projetile.transform.localRotation = transform.rotation;
                projetile.transform.parent = transform;
                charged = true;
                timer = 0;
            }
        }
    }

    public void Fire(Transform navigation, Vector3 to, bool isPlayer)
    {
        if (charged && projetile != null)
        {
            if (navigation != null && isPlayer)
                projetile.GetComponent<motionRocketLaucher>().navigation = navigation;
            if (destination != Vector3.zero)
                projetile.GetComponent<motionProjectile>().destinationPoint = destination;
            if (isPlayer)
                projetile.GetComponent<motionProjectile>().forwardSpeed += GetComponentInParent<motionPlayer>().forwardSpeed;
            else
                projetile.GetComponent<motionProjectile>().forwardSpeed += GetComponentInParent<motionEnemy>().forwardSpeed;
            projetile.GetComponent<motionProjectile>().directionVector = to;
            projetile.GetComponent<motionProjectile>().isPlayers = isPlayer;
            if (projetile.GetComponent<motionRocketLaucher>() != null)
                projetile.GetComponent<motionRocketLaucher>().Fire();
            else
            {
                projetile.GetComponent<motionProjectile>().launch = true;
                projetile.transform.parent = null;
            }

        }
    }
}
