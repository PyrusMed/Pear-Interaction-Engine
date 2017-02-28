using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
	public class GazeHoverHelper : MonoBehaviour
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

		void OnDestroy()
		{
			if (GazeStartEvent != null)
				GazeStartEvent = null;

			if (GazeEndEvent != null)
				GazeEndEvent = null;
		}
	}
}
