using System;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.Events
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
