using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Move : MonoBehaviour {
    private Robot robot;
    public OVRInput.Controller Controller;
    public GameObject player;
    public string url = "http://25.92.54.140/8080";
    void Start () {
        this.robot = new Robot(player);

        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }

    void Update ()
    {

        OVRInput.Update();
        StartCoroutine(GetText());
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
            else if (OVRInput.GetLocalControllerPosition(Controller).x <= 0.2 && OVRInput.GetLocalControllerPosition(Controller).x >= 0.0 && OVRInput.GetLocalControllerPosition(Controller).z >= 0.0)
            {
                player.transform.Rotate(0, 50 * Time.deltaTime, 0);
                //SEND mqtt forward
            }
            else if (OVRInput.GetLocalControllerPosition(Controller).x <= 0.2 && OVRInput.GetLocalControllerPosition(Controller).x >= 0.0 && OVRInput.GetLocalControllerPosition(Controller).z < 0.0)
            {
                player.transform.Rotate(0, -50 * Time.deltaTime, 0);
                //SEND mqtt backward
            }
        }
    }
}
