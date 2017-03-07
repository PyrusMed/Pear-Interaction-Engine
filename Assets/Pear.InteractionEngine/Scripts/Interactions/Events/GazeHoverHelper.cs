using System;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.Events
{
	public class GazeHoverHelper : MonoBehaviour
	{
		public event Action<bool> GazeChanged;

		public void HoverOnGazeStart()
		{
			if(GazeChanged != null)
				GazeChanged(true);
		}

		public void HoverOnGazeEnd()
		{
			if(GazeChanged != null)
				GazeChanged(false);
		}

		void OnDestroy()
		{
			if (GazeChanged != null)
				GazeChanged = null;
		}
	}
}
