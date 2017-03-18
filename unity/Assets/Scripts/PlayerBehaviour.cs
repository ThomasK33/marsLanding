using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {
	private SimpleGameManager gameManager = SimpleGameManager.Instance;

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "SupplyPoint" || col.gameObject.name == "Crater") {
			gameManager.score += gameManager.Supplies.Hit (col.gameObject.transform.position);
			Destroy (col.gameObject);
			gameManager.UpdateText ();
		}
	}
}
