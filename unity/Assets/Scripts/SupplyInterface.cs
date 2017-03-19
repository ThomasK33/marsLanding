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
			mapSupplies = "{\"robot\":{\"x\":576,\"r\":25,\"y\":417},\"points\":[{\"x\":32,\"collected\":false,\"r\":5,\"y\":545,\"score\":-1},{\"x\":22,\"collected\":false,\"r\":5,\"y\":651,\"score\":-1},{\"x\":711,\"collected\":false,\"r\":5,\"y\":873,\"score\":-1},{\"x\":750,\"collected\":false,\"r\":5,\"y\":664,\"score\":-1},{\"x\":722,\"collected\":false,\"r\":5,\"y\":692,\"score\":-1},{\"x\":812,\"collected\":false,\"r\":5,\"y\":133,\"score\":1},{\"x\":136,\"collected\":false,\"r\":5,\"y\":736,\"score\":1},{\"x\":824,\"collected\":false,\"r\":5,\"y\":511,\"score\":1},{\"x\":1170,\"collected\":false,\"r\":5,\"y\":916,\"score\":1},{\"x\":933,\"collected\":false,\"r\":5,\"y\":110,\"score\":1},{\"x\":368,\"collected\":false,\"r\":5,\"y\":596,\"score\":1},{\"x\":226,\"collected\":false,\"r\":5,\"y\":53,\"score\":1},{\"x\":388,\"collected\":false,\"r\":5,\"y\":398,\"score\":1},{\"x\":269,\"collected\":false,\"r\":5,\"y\":414,\"score\":1},{\"x\":1240,\"collected\":false,\"r\":5,\"y\":33,\"score\":1},{\"x\":1048,\"collected\":false,\"r\":5,\"y\":13,\"score\":1},{\"x\":224,\"collected\":false,\"r\":5,\"y\":230,\"score\":1},{\"x\":176,\"collected\":false,\"r\":5,\"y\":394,\"score\":1},{\"x\":877,\"collected\":false,\"r\":5,\"y\":573,\"score\":1},{\"x\":480,\"collected\":false,\"r\":5,\"y\":675,\"score\":1},{\"x\":168,\"collected\":false,\"r\":5,\"y\":827,\"score\":1},{\"x\":881,\"collected\":false,\"r\":5,\"y\":825,\"score\":1},{\"x\":791,\"collected\":false,\"r\":5,\"y\":161,\"score\":1},{\"x\":121,\"collected\":false,\"r\":5,\"y\":857,\"score\":1},{\"x\":193,\"collected\":false,\"r\":5,\"y\":641,\"score\":1},{\"x\":24,\"collected\":false,\"r\":5,\"y\":490,\"score\":1},{\"x\":307,\"collected\":false,\"r\":5,\"y\":136,\"score\":1},{\"x\":427,\"collected\":false,\"r\":5,\"y\":337,\"score\":1},{\"x\":927,\"collected\":false,\"r\":5,\"y\":823,\"score\":1},{\"x\":743,\"collected\":false,\"r\":5,\"y\":120,\"score\":1},{\"x\":870,\"collected\":false,\"r\":5,\"y\":935,\"score\":1},{\"x\":903,\"collected\":false,\"r\":5,\"y\":16,\"score\":1},{\"x\":710,\"collected\":false,\"r\":5,\"y\":592,\"score\":1},{\"x\":375,\"collected\":false,\"r\":5,\"y\":255,\"score\":1},{\"x\":580,\"collected\":false,\"r\":5,\"y\":305,\"score\":1},{\"x\":757,\"collected\":false,\"r\":5,\"y\":930,\"score\":1},{\"x\":1035,\"collected\":false,\"r\":5,\"y\":415,\"score\":1},{\"x\":609,\"collected\":false,\"r\":5,\"y\":881,\"score\":1},{\"x\":523,\"collected\":false,\"r\":5,\"y\":775,\"score\":1},{\"x\":147,\"collected\":false,\"r\":5,\"y\":756,\"score\":1},{\"x\":495,\"collected\":false,\"r\":5,\"y\":644,\"score\":1},{\"x\":455,\"collected\":false,\"r\":5,\"y\":723,\"score\":1},{\"x\":142,\"collected\":false,\"r\":5,\"y\":742,\"score\":1},{\"x\":179,\"collected\":false,\"r\":5,\"y\":117,\"score\":1},{\"x\":884,\"collected\":false,\"r\":5,\"y\":762,\"score\":1},{\"x\":479,\"collected\":false,\"r\":5,\"y\":524,\"score\":1},{\"x\":519,\"collected\":false,\"r\":5,\"y\":359,\"score\":1},{\"x\":298,\"collected\":false,\"r\":5,\"y\":108,\"score\":1},{\"x\":836,\"collected\":false,\"r\":5,\"y\":182,\"score\":1},{\"x\":440,\"collected\":false,\"r\":5,\"y\":476,\"score\":1}],\"world\":{\"x_max\":1280,\"y_max\":960}}";
			if (!random) {
				JsonSerializer serializer = new JsonSerializer ();
				JsonData JSON = JsonConvert.DeserializeObject<JsonData>(mapSupplies);

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
					int score = -1;
					if (n < 8) {
						score = 1;
						this.total++;
					}
					Point point = new Point (x, 0, z, score, false);
					this.points.Add (point);
				}
			}
		}

		/*public SupplyInterface (float maxX, float maxZ, string mapSupplies)
		{
			SupplyInterface (
				maxX, 
				maxZ,
				mapSupplies, 
				false);
		}*/

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

class JsonData {
	public IPoint[] points { get; set; }
	public IRobot robot { get; set; }
	public IWorld world { get; set; }
}