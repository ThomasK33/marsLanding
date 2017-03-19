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

		public List<Point> Points {
			get { return this.points; }
		}

		public int Total {
			get { return this.total; }
		}

		public int Found {
			get { return this.found; }
		}


        public class MapRobot
        {
            public bool collected;
            public int r;
            public int x;
            public int score;
            public int y;
        }

        public SupplyInterface (float maxX, float maxZ, string mapSupplies)
		{
            mapSupplies = "{\"points\": [{\"collected\": false, \"r\": 5, \"x\": 908, \"score\": 1, \"y\": 831},{\"collected\": false, \"r\": 5, \"x\": 100, \"score\": -1, \"y\": 200},{\"collected\": true, \"r\": 5, \"x\": 600, \"score\": 1, \"y\": 370}]}";
            List<MapRobot> steps = JsonConvert.DeserializeObject<List<MapRobot>>(mapSupplies);
			JsonSerializer serializer = new JsonSerializer ();
			IJSON JSON = serializer.Deserialize (new JsonReader (mapSupplies));

			foreach (IPoint point in JSON.points) {
				Point newPoint = new Point(point.x, 0, point.y, point.score, point.collected);
				points.Add (newPoint);
			}
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

interface IPoint {
	int x { get; set; }
	int y { get; set; }
	int r { get; set; }
	int score { get; set; }
	bool collected { get; set; }
}

interface IJSON {
	IPoint[] points { get; set; }
}