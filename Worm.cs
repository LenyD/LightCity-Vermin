using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Worm : MonoBehaviour {
	float vit = 1.25f;
	public GameObject centerpoint;
	public Vector3 sens = Vector3.up;
	public LayerMask Mask;
	public GameObject game;
	float ppValue;

	Animation anim;
	//pour changer l'amplitude des cercle, changer le z du point central dans le prefab
	// Use this for initialization
	void Awake () {
		anim = GetComponent <Animation> ();
	}

	void Start () {
	
	}

	void OnTriggerStay(Collider hitBox){
		//Appelé à l'entré d'une hitBox trigger(les hitBox de visions des gardes)
		Transform pTransform = hitBox.transform;
		Vector3 headPos = new Vector3(pTransform.position.x,pTransform.position.y+1.5f,pTransform.position.z);
		Vector3 dir = transform.position - headPos ;
		Physics.Raycast (headPos, dir);	
		Debug.DrawRay (headPos, dir);
		RaycastHit rayHit;
		Ray ray = new Ray(headPos, dir); //fait un rayon du worm vers le garde.
		if (Physics.Raycast (ray, out rayHit)){// si le rayon touche quelque chose
			//print(rayHit.collider.gameObject.layer);
			if (rayHit.collider.gameObject.layer == LayerMask.NameToLayer ("Worm")) {//si ce que le rayon touche est le worm, il est en vue du garde
				hitBox.GetComponentInChildren<Light> ().color = Color.Lerp (hitBox.GetComponentInChildren<Light> ().color,Color.red,0.5f/Vector3.Distance(transform.position,hitBox.transform.position) );//change la couleur de la lumière du garde
				hitBox.SendMessage("follow",transform.position);//Envoie un message d'activé la fonction follow
				if (Vector3.Distance (transform.position, hitBox.transform.position) <= 1.5f) {//si le garde est trop proche
					game.SendMessage("resetGame");//reset le jeu
				}
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		

		if (!Input.GetMouseButton (0) && !Input.GetMouseButton (1)) {//Si aucun bouton est appuyé
			anim.Play ("idle");

		} else {
			if (Input.GetMouseButton (0) && Input.GetMouseButton (1)) {//si les deux sont appuyé
				anim.Play ("crawlFast");
				goGauche (3);
				goDroite (3);
			} else {
				anim.Play ("crawl");
				if (Input.GetMouseButton (1)) {//si les left click
					goGauche (5);

				}
				if (Input.GetMouseButton (0)) {//si les right click
					goDroite (5);

				}
			}


		}
			
	}

	void changerSens(){// inverse le sens de rotation et la position du point central
		sens.y = -sens.y;
		centerpoint.transform.localPosition = new Vector3((centerpoint.transform.localPosition.x*-1) ,0,0);
	}

	void goGauche(float num){//tourne vers la gauche autour du point central
		sens.y = Vector3.up.y;
		centerpoint.transform.localPosition = new Vector3(num,0,0);
		transform.RotateAround (centerpoint.transform.position, sens, vit);
	}
	void goDroite(float num){//tourne vers la droite autour du point central
		sens.y = -Vector3.up.y;
		centerpoint.transform.localPosition = new Vector3(-num,0,0);
		transform.RotateAround (centerpoint.transform.position, sens, vit);
	}

}
