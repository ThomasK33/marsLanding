using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace AssemblyCSharp
{
    public class SupplyInterface
	{
		private List<Point> points = new List<Point>();
		private int total = 0;
		private int found = 0;

		private int world_max_x;
		private int world_max_z;
		private int robot_x;
		private int robot_z;

		public List<Point> Points {
			get { return this.points; }
		}

		public int Total {
			get { return this.total; }
		}

		public int Found {
			get { return this.found; }
		}

		public int WorldMaxX {
			get { return this.world_max_x; }
		}

		public int WorldMaxZ {
			get { return this.world_max_z; }
		}

		public int RobotX {
			get { return this.robot_x; }
		}

		public int RobotZ {
			get { return this.robot_z; }
		}

		public SupplyInterface (float maxX, float maxZ, string mapSupplies, bool random)
		{
            // mapSupplies = "{\"points\": [{\"collected\": false, \"r\": 5, \"x\": 908, \"score\": 1, \"y\": 831},{\"collected\": false, \"r\": 5, \"x\": 100, \"score\": -1, \"y\": 200},{\"collected\": true, \"r\": 5, \"x\": 600, \"score\": 1, \"y\": 370}]}";
			if (!random) {
				JsonSerializer serializer = new JsonSerializer ();
				IJSON JSON = serializer.Deserialize (new JsonReader (mapSupplies));

				robot_x = JSON.robot.x;
				robot_z = JSON.robot.y;
				world_max_x = JSON.world.x_max;
				world_max_z = JSON.world.y_max;
				foreach (IPoint point in JSON.points) {
					Point newPoint = new Point (point.x, 0, point.y, point.score, point.collected);
					points.Add (newPoint);
				}
			} else {
				robot_x = 305;
				robot_z = 212;
				System.Random rand = new System.Random ();
				for (int i = 0; i < 45; i++) {
					int x = rand.Next (1, (int)maxX);
					int z = rand.Next(1, (int)maxZ);
					int n = rand.Next (1, 11);
					int score = 1;
					if (n < 8) {
						score = 1;
						this.total++;
					}
					Point point = new Point (x, 0, z, score, false);
					this.points.Add (point);
				}
			}
		}

		public SupplyInterface (float maxX, float maxZ, string mapSupplies)
		{
			SupplyInterface (maxX, maxZ, mapSupplies, false);
		}

		public int Hit (Vector3 pos) {
			foreach (Point point in points) {
				if (point.X == pos.x && point.Z == pos.z) {
					point.collected = true;
					if (point.Score > 0) {
						found--;
					}
					return point.Score;
				}
			}
			return 0;
		}

	}

	public class Point
	{
		public bool collected = false;
		private int x;
		private int z;
		private int y;
		private int score;

		public int X {
			get { return this.x; }
		}

		public int Z {
			get { return this.z; }
		}

		public int Y {
			get { return this.y; }
		}

		public int Score {
			get { return this.score; }
		}

		public Point(int x, int y, int z, int score, bool collected)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.score = score;
			this.collected = collected;
		}
	}
}

interface IRobot {
	int r { get; set; }
	int x { get; set; }
	int y { get; set; }
}

interface IWorld {
	int x_max { get; set; }
	int y_max { get; set; }
}

interface IPoint {
	int x { get; set; }
	int y { get; set; }
	int r { get; set; }
	int score { get; set; }
	bool collected { get; set; }
}

interface IJSON {
	IPoint[] points { get; set; }
	IRobot robot { get; set; }
	IWorld world { get; set; }
}