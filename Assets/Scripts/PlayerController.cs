using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

	// player movement variables
	public float speed;
	public float tilt;
	public Boundary boundary;
	//private float dodgeSpeed = 0.5f;

	// player shooting variables
	public GameObject shot;
	public Transform frontShotSpawn;
	public float fireRate = 0.5f;
	private float nextFire = 0.0f;

	void Update() 
	{
		// player shooting code
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
					nextFire = Time.time + fireRate;
					Instantiate (shot, frontShotSpawn.position, frontShotSpawn.rotation);
					audio.Play();
		}
		
		// player dodge/barrel roll mechanic
		//if (Input.GetKeyDown(KeyCode.Q)){transform.Rotate(0, 0.5f, 0);}
		
		//if (Input.GetKeyDown(KeyCode.E)){transform.Rotate(-Vector3.up * dodgeSpeed * Time.deltaTime);}	

	}

	void FixedUpdate(){
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");


		// player movement code
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;

		rigidbody.position = new Vector3 
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
			);

		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
		}

}
