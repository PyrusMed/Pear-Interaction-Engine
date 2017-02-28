using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
	public class HoverOnGazeHelper : MonoBehaviour
	{

		public event Action GazeStartEvent;
		public event Action GazeEndEvent;

		public void HoverOnGazeStart()
		{
			GazeStartEvent();
		}

		public void HoverOnGazeEnd()
		{
			GazeEndEvent();
		}
	}
}
