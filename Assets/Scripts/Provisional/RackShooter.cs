using UnityEngine;
using System.Collections;

public class RackShooter : MonoBehaviour {

	public float fireRate;
	private float nextFire = 0.0f;

	public float burstFireRate;
	public int numberOfBullets;
	public Transform spawnPoint;
	public float projectileOffset;

	public GameObject projectile;
	public GameObject launchEffect;

	private GameObject target;
	public float minRange;
	public float maxRange;
	private float distance;
	
	void Update () {
		target = GameObject.FindGameObjectWithTag("Player");
		distance = Vector3.Distance(transform.position,target.transform.position);
		if ((distance > minRange) && (distance < maxRange)) {
				if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				StartCoroutine (Shoot ());
				}
		}
	}
	
	IEnumerator Shoot() {
		for (int i = 0; i < numberOfBullets; i++)
		{
			Vector3 position = new Vector3(spawnPoint.position.x + i*projectileOffset,
			                               spawnPoint.position.y,
			                               spawnPoint.position.z);

			Instantiate(projectile, position, spawnPoint.rotation);
			Instantiate(launchEffect, position, spawnPoint.rotation);
			yield return new WaitForSeconds(burstFireRate);
		}
	}
}
