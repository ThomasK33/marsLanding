using System.Collections;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Timers;
using System;

public class SimpleGameManager {
	protected SimpleGameManager() {
		
	}
    private string mapSupplies;
	private static SimpleGameManager instance = null;

	public static SimpleGameManager Instance {
		get {
			if (SimpleGameManager.instance == null) {
				SimpleGameManager.instance = new SimpleGameManager ();
			}
			return SimpleGameManager.instance;
		}
	}

	public GameObject mainSphere;
	public GameObject mainCrater;

	private SupplyInterface supplies;
	private Terrain terrain;
	public int score = 0;
	private int secondsRemaining;

	private Timer timer;

	public SupplyInterface Supplies {
		get { return this.supplies; }
	}
		
	public int SecondsRemaining {
		get { return secondsRemaining; }
	}

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

    public void Setup () {
		score = 0;
		secondsRemaining = 120;
		this.mainSphere = GameObject.FindGameObjectWithTag ("mainSphere");
        this.mainCrater = GameObject.FindGameObjectWithTag("mainCrater");
		this.terrain = UnityEngine.Object.FindObjectOfType<Terrain> ();
		Vector3 size = this.terrain.terrainData.size;

        StartCoroutine(WaitForRequest(new WWW("http://localhost:6666/players/oculUS/game")));
        while (mapSupplies.Length == 0) ;

        this.supplies = new SupplyInterface (size.x, size.z, mapSupplies);
		PlaceSupplies ();

		timer = new Timer (1000);
		timer.Elapsed += HandleTimer;
		timer.Start ();
	}

	private void PlaceSupplies() {
		foreach (Point point in this.supplies.Points) {
			GameObject newPoint;
			if (point.Score == -1) {
				newPoint = UnityEngine.Object.Instantiate (this.mainCrater);
				newPoint.name = "Crater";
			} else {
				newPoint = UnityEngine.Object.Instantiate (this.mainSphere);
				newPoint.name = "SupplyPoint";
			}
			Vector3 position = new Vector3 (point.X, 0.5f, point.Z);
			float y = this.terrain.SampleHeight (position);
			position.y = y + 5f;
			newPoint.transform.position = position;
		}
	}

	private void HandleTimer(object sender, EventArgs args) {
		secondsRemaining--;
		if (secondsRemaining == 0) {
			timer.Stop ();
		}
	}

	public void UpdateText() {
		Text scoreText = GameObject.Find ("scoreText").GetComponent<Text> ();
		Text timeText = GameObject.Find ("timerText").GetComponent<Text> ();
		Text foundText = GameObject.Find ("foundText").GetComponent<Text> ();
		if (supplies != null) {
			scoreText.text = "Score: " + score;
			timeText.text = "Seconds remaining: " + secondsRemaining + "s";
			foundText.text = "Found: " + supplies.Found + " / " + supplies.Total;
		}
	}
}
