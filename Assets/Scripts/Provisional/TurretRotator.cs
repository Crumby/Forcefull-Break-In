using UnityEngine;
using System.Collections;

public class TurretRotator : MonoBehaviour {

	public GameObject target;

	void Update () {
		target = GameObject.FindGameObjectWithTag("BullsEye");
		transform.LookAt (target.transform);
	}
}