using UnityEngine;
using System.Collections;

public class tester : MonoBehaviour {

	public GameObject explosion;

	// Use this for initialization
	void Start () {
	
	}
	void OnCollisionEnter(Collision collision){
		if(collision.transform.GetComponent<shipSystemsPlayer>()!=null){
			collision.transform.GetComponent<shipSystemsPlayer>().recieveDmg(float.MaxValue,collision.contacts[0].point);
			Debug.LogError("P");
		}
		else if(collision.transform.GetComponent<shipSystemsEnemy>()!=null){
			collision.transform.GetComponent<shipSystemsEnemy>().recieveDmg(float.MaxValue,collision.contacts[0].point);
			Debug.LogError("E");
		}
		else {
			Destroy(collision.gameObject);
			Instantiate(explosion,collision.contacts[0].point,Quaternion.identity);
			Debug.LogError("O");
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
