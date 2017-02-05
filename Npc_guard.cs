using UnityEngine;
using System.Collections;

public class Npc_guard : MonoBehaviour {

	//public Transform[] path; pour le déplacement avec way points
	public float vit = 1f;
	//public int goalPoint = 0; pour le déplacement avec way points
	public float length = 5f;//longeur de vérification pour les murs
	public GameObject joueur;

	int randDir = 0;
	float rotSpeed = 0.05f;

	// Use this for initialization
	void Awake(){
	}

	void Start () {
		Debug.Log (this);
	}

	void turnToward(Vector3 point){
		Vector3 lookTarget = transform.position+point; 	//détermine le point de regard final
		lookTarget.y = transform.position.y;			//garde le y fixe
		Quaternion initialRot = transform.rotation;		//enregistre la position d'origine;
		transform.LookAt (lookTarget);					//regarde la cible
		Quaternion targetRot = transform.rotation;		//enregiste la rotation finale
		transform.rotation = initialRot;				//revien à la rotation initial

		transform.rotation = Quaternion.Slerp (initialRot, targetRot, 1 * rotSpeed);// applice le Slerp de la rotation initial à finale
	}

	void follow(Vector3 target){
		transform.LookAt (target);		//regarde la cible en paramettre
	}

	void FixedUpdate () {
		//Déplacement avec way point
		/*if (path.Length >= 1) {
			float distance = Vector3.Distance (path [goalPoint].position, transform.position);
			transform.position = Vector3.MoveTowards (transform.position, path [goalPoint].position, vit * 0.01f);
			Vector3 lookTarget = path [goalPoint].position;
			lookTarget.y = transform.position.y;
			Quaternion initialRot = transform.rotation;
			transform.LookAt (lookTarget);
			Quaternion targetRot = transform.rotation;
			transform.rotation = initialRot;
			float rotSpeed = 0.1f;
			transform.rotation = Quaternion.Slerp (initialRot, targetRot, 1 * rotSpeed);

			if (distance <= vit * 0.005f) {
				goalPoint++;

			}
			if (goalPoint >= path.Length) {
				goalPoint = 0;
			}
		}*/
		GetComponentInChildren<Light> ().color = Color.Lerp ( GetComponentInChildren<Light> ().color,Color.yellow,0.004f);//revien ver la couleur initial de flashlight
		Vector3 forward = transform.TransformDirection(Vector3.forward) * length; 	//ligne droit devant
		Vector3 forward1 = transform.TransformDirection(Vector3.right) * length;	
		forward1 = ((forward + forward1) / 2f)*1.14f;								//ligne diagonale à droite
		Vector3 forward2 = transform.TransformDirection(Vector3.left) * length;		
		forward2 = ((forward + forward2) / 2f)*1.14f;								//ligne diagonale à gauche

		Debug.DrawRay(transform.position+Vector3.up,forward1, Color.green);
		Debug.DrawRay(transform.position+Vector3.up,forward2, Color.red);
		Debug.DrawRay (transform.position+Vector3.up, forward,Color.cyan);

		transform.position = Vector3.MoveTowards (transform.position, transform.position+forward, vit * 0.01f);//Avance le garde

		RaycastHit rayHit;
		Ray ray = new Ray(transform.position+Vector3.up, forward);
		Ray ray1 = new Ray(transform.position+Vector3.up, forward1);
		Ray ray2 = new Ray(transform.position+Vector3.up, forward2);

		if (Physics.Raycast (ray, out rayHit,length)){			//si il y a une collision avec le rayon avant
			if (!Physics.Raycast (ray1, out rayHit, length)) {	//est-ce que la droite est libre?
				turnToward (forward1);							//tourne vers la droite tant que les conditions sont true
			} else if (!Physics.Raycast (ray2, out rayHit, length)) {//est-ce que la gauche est libre?				
				turnToward (forward2);							//tourne vers la droite tant que les conditions sont true
			}else{												//si la droite et la gauche sont bloqué.
				if (randDir == 0) {								//Si la direction random == 0
					randDir = Mathf.FloorToInt(Random.Range(1,3));//Choisir une direction entre 1-2 au hasard
				}
				if (randDir == 1) {		//tourne d'un côté choisis au hasard
					turnToward (forward1);
				} else {
					turnToward (forward2);
				}
			}

		}else if(Physics.Raycast (ray1, out rayHit, length*1.14f) && !Physics.Raycast (ray2, out rayHit, length*1.14f)) {//si il y a une collision avec seulement la un côté
			turnToward (forward2);								//dévis vers l'autre côté
			randDir = 0;										//reset la direction au hasard
		}else if(Physics.Raycast (ray2, out rayHit, length*1.14f) && !Physics.Raycast (ray1, out rayHit, length*1.14f)) {
			turnToward (forward1);
			randDir = 0;
		}
			
	}
		
}
