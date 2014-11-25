using UnityEngine;
using System.Collections;

public class MultiRackShooter : MonoBehaviour {
	
	public float fireRate;
	private float nextFire = 0.0f;
	
	public float burstFireRate;
	public int gunsInLine;
	public int gunsInColumn;
	public Transform spawnPoint;
	public float projectileOffsetX;
	public float projectileOffsetY;
	
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
				StartCoroutine (SetToShoot ());
			}
		}
	}

	IEnumerator SetToShoot(){
			for (int j = 0; j < gunsInColumn; j++) {
				StartCoroutine (Shoot (j));
				yield return new WaitForSeconds (gunsInLine * burstFireRate);
			}
		}
	
	IEnumerator Shoot(int rows) {
						for (int i = 0; i < gunsInLine; i++) {
								Vector3 position = new Vector3 (spawnPoint.position.x + i * projectileOffsetX,
			                               spawnPoint.position.y + rows * projectileOffsetY,
			                               spawnPoint.position.z);
			
								Instantiate (projectile, position, spawnPoint.rotation);
								Instantiate (launchEffect, position, spawnPoint.rotation);
								yield return new WaitForSeconds (burstFireRate);
						}

	}
}
