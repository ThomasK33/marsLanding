using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour {
	private SimpleGameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = SimpleGameManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		gameManager.UpdateText ();
		if (gameManager.SecondsRemaining == 0) {
			gameManager.Setup ();
		}
	}
}
