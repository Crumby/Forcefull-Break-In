using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
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
		if (other.tag == "Boundary") 
		{
			return;
				}
		Instantiate (explosion, transform.position, transform.rotation);
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			theOverlord.GameOver();
		}
		theOverlord.AddScore (scoreValue);
		Destroy (other.gameObject);
		Destroy (gameObject);
	}

}
