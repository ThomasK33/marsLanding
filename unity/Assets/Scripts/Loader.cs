using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public SimpleGameManager gameManager;

	void Awake () {
		gameManager = SimpleGameManager.Instance;
		gameManager.mainCrater = GameObject.FindGameObjectWithTag ("mainCrater");
		gameManager.mainSphere = GameObject.FindGameObjectWithTag ("mainSphere");
		gameManager.Setup ();
	}
}
