using System.Collections;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Timers;
using System;

public class SimpleGameManager {
	protected SimpleGameManager() {
		
	}
    public string mapSupplies;
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

    public void Setup (string mapSupplies) {
		score = 0;
		secondsRemaining = 120;
		this.mainSphere = GameObject.FindGameObjectWithTag ("mainSphere");
        this.mainCrater = GameObject.FindGameObjectWithTag("mainCrater");
		this.terrain = UnityEngine.Object.FindObjectOfType<Terrain> ();
		Vector3 size = this.terrain.terrainData.size;

        this.supplies = new SupplyInterface (size.x, size.z, mapSupplies, true);
		PlaceSupplies ();
		PlaceRobot (this.supplies.RobotX, this.supplies.RobotZ);
		timer = new Timer (1000);
		timer.Elapsed += HandleTimer;
		timer.Start ();
	}

	public void Setup () {
		this.Setup (mapSupplies);
	}

	private void PlaceSupplies() {

		int width = terrain.terrainData.heightmapWidth;
		int height = terrain.terrainData.heightmapHeight;

		foreach (Point point in this.supplies.Points) {
			GameObject newPoint;

			Vector3 position = new Vector3 (point.X, 0.5f, point.Z);
			float y = this.terrain.SampleHeight (position);
			position.y = y + 5f;

			if (point.Score == -1) {
				newPoint = UnityEngine.Object.Instantiate (this.mainCrater);
				newPoint.name = "Crater";
				newPoint.transform.position = position;
				
				/*int size = 50;
				int desired = (int) (y - 2f);

				Vector3 tempCoord = (newPoint.transform.position - terrain.gameObject.transform.position);
				Vector3 coord;
				coord.x = tempCoord.x / terrain.terrainData.size.x;
				coord.y = tempCoord.y / terrain.terrainData.size.y;
				coord.z = tempCoord.z / terrain.terrainData.size.z;

				int posXInTerrain = (int)(coord.x * width);
				int posYInTerrain = (int)(coord.y * height);
				int offset = size;
				int offsetY = (posYInTerrain - offset);
				int offsetX = (posXInTerrain - offset);
				if (offsetY < 1) {
					offsetY = 1;
				}
				if (offsetX < 1) {
					offsetX = 1;
				}

				float[,] heights = terrain.terrainData.GetHeights (posXInTerrain - offset, posYInTerrain - offset, size, size);
				for (int i = 0; i < size; i++) {
					for (int j = 0; j < size; j++) {
						heights [i, j] = desired;
					}
				}
				desired += (int)Time.time;

				terrain.terrainData.SetHeights (posXInTerrain - offset, posYInTerrain - offset, heights);*/
			} else {
				newPoint = UnityEngine.Object.Instantiate (this.mainSphere);
				newPoint.name = "SupplyPoint";
				newPoint.transform.position = position;
			}


		}
	}

	private void PlaceRobot(int x, int z) {
		try {
			GameObject robot = GameObject.Find ("player");
			Vector3 newPosition = new Vector3(x, 0.5f, z);
			newPosition.y = this.terrain.SampleHeight(newPosition) + 7f;
			robot.transform.position = newPosition;
		} catch (Exception e) {

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
