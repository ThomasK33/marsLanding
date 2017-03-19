using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class Move : MonoBehaviour {
    private Robot robot;
    public OVRInput.Controller Controller;
    public GameObject player;
    string url = "https://www.google.com/";
    WWW www;
    void Start () {
        this.robot = new Robot(player);

    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.data);
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }


    void Update ()
    {
        OVRInput.Update();

       
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log(OVRInput.GetLocalControllerPosition(Controller).x);
            if (OVRInput.GetLocalControllerPosition(Controller).x <= 0)
            {
                player.transform.Rotate(0, -25 * Time.deltaTime, 0);
                //StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/left")));
                //SEND mqtt turn left
            }
            else if (OVRInput.GetLocalControllerPosition(Controller).x > 0)
            {
                player.transform.Rotate(0, 25 * Time.deltaTime, 0);
                //StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/right")));
                //SEND mqtt turn right
            }
            else if (OVRInput.Get(OVRInput.Button.One))
            {
                player.transform.position += player.transform.forward * Time.deltaTime * 1;
                //StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/forward")));
                //SEND mqtt forward
            }
            else if (OVRInput.Get(OVRInput.Button.Three))
            {
                player.transform.position += player.transform.forward * Time.deltaTime * -1;
                //StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/backward")));
                //SEND mqtt backward
            }
        }
    }
}
