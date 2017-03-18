using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Robot
	{
		private GameObject robot;
        public static int movement = 0;

        public Robot (GameObject robot)
		{
			this.robot = robot;
		}

		/**
		 * Rotate robot to the right
		 * @param float degrees The amount the robot should turn to the right
		 * @return float Actual degrees turned
		 */
		public float RotateRight(float amount)
		{
			System.Random rand = new System.Random();
			int randInt = rand.Next(1, 3);
			int off = rand.Next(1, 11);

			if (randInt == 1) {
				amount -= amount / 100 * off;
			} else {
				amount += amount / 100 * off;
			}

			this.robot.transform.Rotate (0, amount, 0);

            return amount;
		}

		/**
		 * Rotate robot to the left
		 * @param float degrees The amount the robot should turn to the left
		 * @return float Actual degrees turned
		 */
		public float RotateLeft(float amount)
		{
			System.Random rand = new System.Random();
			int randInt = rand.Next(1, 3);
			int off = rand.Next(1, 11);

            

			if (randInt == 1) {
				amount -= amount / 100 * off;
			} else {
				amount += amount / 100 * off;
			}

            
			this.robot.transform.Rotate (0, amount * -1, 0);

            //	Debug.Log (this.robot.transform.position);
            return amount;
		}

		public void MoveForwards(float speed)
		{
			this.robot.transform.position += this.robot.transform.forward * Time.deltaTime * speed;
		}

		public void MoveBackwards(float speed)
		{
			this.robot.transform.position -= this.robot.transform.forward * Time.deltaTime * speed;
		}
	}
}

