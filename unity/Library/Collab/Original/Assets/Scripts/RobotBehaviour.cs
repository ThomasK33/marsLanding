using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class RobotBehaviour : MonoBehaviour {

	public GameObject robotGameObject; 
	public ParticleSystem dust;
	private Robot robot;
	private int count = 0;

	// Use this for initialization
	void Start () {
		this.robot = new Robot(this.gameObject);
		var emission = this.dust.emission;
		emission.enabled = true;
	}
	
	
}
