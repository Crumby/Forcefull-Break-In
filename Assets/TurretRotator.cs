using UnityEngine;
using System.Collections;

public class TurretRotator : MonoBehaviour {

	public Transform target;

	void Update () {
		transform.LookAt (target);
	}
}