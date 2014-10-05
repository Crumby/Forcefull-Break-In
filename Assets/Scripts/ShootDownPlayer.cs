using UnityEngine;
using System.Collections;

public class ShootDownPlayer : MonoBehaviour {

	public GameObject playerExplosion;
	private TheOverlord theOverlord;
	
	void Start()
	{
		GameObject theOverlordObject = GameObject.FindWithTag ("GameController");
		if(theOverlordObject != null){
			theOverlord = theOverlordObject.GetComponent<TheOverlord>();
		}
		if (theOverlordObject == null) 
		{
			Debug.Log("Overlord script not found!");
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			theOverlord.GameOver();
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}

}
