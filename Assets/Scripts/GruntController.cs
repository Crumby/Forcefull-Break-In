using UnityEngine;
using System.Collections;

public class GruntController : MonoBehaviour {

	public GameObject gruntShot;
	public Transform gruntShotSpawn;
	public float fireRate;
	private float nextFire = 0.0f;

	void Update() 
	{
		// Grunt shooting code
		if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				Instantiate (gruntShot, gruntShotSpawn.position, gruntShotSpawn.rotation);
				audio.Play ();
		}
	}
}
