using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private Robot robot;
    public OVRInput.Controller Controller;
    public GameObject player;
    
    void Start () {
        this.robot = new Robot(player);
    }
	
	void Update ()
    {

        OVRInput.Update();
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log(OVRInput.GetLocalControllerPosition(Controller).x);
            if (OVRInput.GetLocalControllerPosition(Controller).x < -0.4)
            {
                player.transform.Rotate(0, -50 * Time.deltaTime, 0);
                //SEND mqtt turn left
            }
            else if (OVRInput.GetLocalControllerPosition(Controller).x > 0.2)
            {
                player.transform.Rotate(0, 50 * Time.deltaTime, 0);
                //SEND mqtt turn right
            }
            else if (OVRInput.GetLocalControllerPosition(Controller).x < 0.1 && OVRInput.GetLocalControllerPosition(Controller).z == 0.0)
            {
                player.transform.Rotate(0, 50 * Time.deltaTime, 0);
                //SEND mqtt forward
            }
            else if (OVRInput.GetLocalControllerPosition(Controller).x == 0.0 && OVRInput.GetLocalControllerPosition(Controller).z == -0.1)
            {
                player.transform.Rotate(0, -50 * Time.deltaTime, 0);
                //SEND mqtt backward
            }
        }
    }
}
