using UnityEngine;
using System.Collections;

public class StandardShooter : MonoBehaviour {

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;

	public Transform target;
	public float minRange;
	public float maxRange;
	private float distance;

	void Update() 
	{
		distance = Vector3.Distance(transform.position,target.position);
		if((distance > minRange) && (distance < maxRange))
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
				//audio.Play ();
		}
	}
}
