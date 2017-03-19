using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public SimpleGameManager gameManager;

	private string mapSupplies;
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			mapSupplies = www.data;
		}
		else
		{
			Debug.Log("Error: " + www.error);
		}
	}

	void Awake () {
		gameManager = SimpleGameManager.Instance;
		gameManager.mainCrater = GameObject.FindGameObjectWithTag ("mainCrater");
		gameManager.mainSphere = GameObject.FindGameObjectWithTag ("mainSphere");
		StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/players/oculUS/game")));
		while (mapSupplies.Length == 0) ;
		gameManager.Setup (mapSupplies);
	}
}
