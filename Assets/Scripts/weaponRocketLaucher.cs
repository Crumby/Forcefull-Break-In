using UnityEngine;
using System.Collections;

public class weaponRocketLaucher : MonoBehaviour
{
    public GameObject projectile;
	public Vector3 destination {get;set;}

    // Use this for initialization
    void Start()
    {
		destination=Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(Vector3 to, bool isPlayer)
    {
		Quaternion direction=Quaternion.identity;
		if(destination!=Vector3.zero)
			direction=Quaternion.LookRotation(Vector3.Normalize(destination-transform.position));
        var hlp = ((GameObject)Instantiate(projectile, transform.position,direction)).GetComponent<motionProjectile>();
		if(isPlayer)
			hlp.GetComponent<motionProjectile>().forwardSpeed+=GetComponentInParent<motionPlayer>().forwardSpeed;
		else hlp.GetComponent<motionProjectile>().forwardSpeed+=GetComponentInParent<motionEnemy>().forwardSpeed;
        hlp.direction = to;
        hlp.isPlayers = isPlayer;
    }
}
