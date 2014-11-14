using UnityEngine;
using System.Collections;

public class StandardShot : MonoBehaviour {

	public Transform target;
	public float speed = 3f;
	private bool ifTarget = false;
	public void SetTarget(Transform helTarg){
		target = helTarg;
		ifTarget = true;
	}
	void Update () {
		if(ifTarget){
			Vector3 toTarget = target.position - transform.position;
			rigidbody.velocity = toTarget.normalized * speed;
			transform.forward = toTarget.normalized;
		}
	}
}