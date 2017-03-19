using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject body;

    private Vector3 offset;
    
    // Use this for initialization
    void Start () {
        offset = transform.position - body.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = body.transform.position + offset;
        //transform.rotation = body.transform.rotation;
	}
}
