using UnityEngine;
using System.Collections;

public class TrackTarget : MonoBehaviour {

	public Transform target;

	public GameObject projectile;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;

	public float range;

	void Update () {
		transform.LookAt (target);

		// fire only if target in range
		float distance = Vector3.Distance (transform.position, target.transform.position);
		if(distance <= range){
			// continuous fire
			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				Instantiate (projectile, shotSpawn.position, shotSpawn.rotation);
				audio.Play ();
			}
		}
	}
}
