using UnityEngine;
using System.Collections;

public class CameraSlerpFollow : MonoBehaviour {

	public Transform target;
	public Transform position;
	// Use this for initialization
	void Start () {
		transform.position = position.position;
		transform.LookAt (target.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Suit une cible prédéfinis en tournant de facon smooth et en restant sur une position, la position et la cible peuvent être en mouvement
		transform.position = position.position;
		Vector3 lookTarget = target.position;
		lookTarget.y = transform.position.y;
		Quaternion initialRot = transform.rotation;
		transform.LookAt (lookTarget);
		Quaternion targetRot = transform.rotation;
		transform.rotation = initialRot;
		float rotSpeed = 0.1f;
		transform.rotation = Quaternion.Slerp (initialRot, targetRot, 1 * rotSpeed);

	}
}
