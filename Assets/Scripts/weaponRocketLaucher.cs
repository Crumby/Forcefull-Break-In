using UnityEngine;
using System.Collections;

public class weaponRocketLaucher : MonoBehaviour
{
    public GameObject projectile;
    public Quaternion rotation { get; set; }

    // Use this for initialization
    void Start()
    {
        rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(Vector3 to, bool isPlayer)
    {
        var hlp = ((GameObject)Instantiate(projectile, transform.position, rotation)).GetComponent<motionProjectile>();
        hlp.direction = to;
        hlp.isPlayers = isPlayer;
    }
}
