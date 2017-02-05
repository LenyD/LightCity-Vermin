using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemaster : MonoBehaviour {
	// Use this for initialization

	bool reset = false;
	bool openMenu=true;
	Scene startingScene;

	void Start () {
		Cursor.visible = false;
		startingScene = SceneManager.GetActiveScene ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (reset) {
			//Application.LoadLevel (Application.loadedLevel);
			SceneManager.LoadScene(startingScene.name);
		}

		if (Input.GetKeyDown (KeyCode.Escape) == true) {
			if (openMenu) {//open it
				openMenu = false;
				Cursor.visible = true;
			} else {
				openMenu = true;
				Cursor.visible = false;
			}
		}
	}

	void OnGUI(){
		if (!openMenu) {
			if (GUI.Button (new Rect (Screen.width/2 -50 , Screen.height/2, 100, 25), "Quit")) {
				Application.Quit();
			}
			if (GUI.Button (new Rect (Screen.width/2 -50 , Screen.height/2+35, 100, 25), "Return")) {
				openMenu = true;
				Cursor.visible = false;
			}
		}
	}

	void resetGame(){
		reset = true;
	}
}
