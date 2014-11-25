using UnityEngine;
using System.Collections;

public class Lvl2BossTurretRotator : MonoBehaviour {
	
	private GameObject target;
	
	void Update () {
		target = GameObject.FindGameObjectWithTag("Player");
		transform.LookAt (target.transform);
	}
}