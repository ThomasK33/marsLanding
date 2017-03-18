using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		public SupplyInterface (float maxX, float maxZ)
		{
			System.Random rand = new System.Random();
			for (int i = 0; i < 45; i++) {
				int x = rand.Next (1, (int)maxX);
				int z = rand.Next (1, (int)maxZ);
				int n = rand.Next (1, 3);
				int score = -1;
				if (n == 1) {
					score = 1;
					this.total++;
				}
				Point point = new Point (x, z, 0, score);
				this.points.Add (point);
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

		public Point(int x, int z, int y, int score)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.score = score;
		}
	}
}

