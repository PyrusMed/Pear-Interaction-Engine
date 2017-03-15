using Pear.InteractionEngine.Controllers;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Enables or disables the leap motion controllers based on other controllers in the same hand
	/// </summary>
	public class EnableDisableLeapControllerBasedOnOtherControllers : MonoBehaviour
	{

		[Tooltip("Left controller")]
		public LeapMotionController Left;

		[Tooltip("Right controller")]
		public LeapMotionController Right;

		// Use this for initialization
		void Awake()
		{
			// Loop over each controller and hook up events to non-leap motion controllers
			// that occupy the same hands
			ControllerManager.Instance.ControllerAddedEvent += controller =>
			{
				// Ignore leap motion controllers
				if (controller is LeapMotionController)
					return;

				// Get the correct leap controller
				LeapMotionController leapController = null;
				if (controller.Location == Left.Location)
					leapController = Left;
				else if (controller.Location == Right.Location)
					leapController = Right;

				// If there's a leap controller in the same location as another controller
				// make sure the leap controller is not showing when the other controller is enabled
				if (leapController != null)
				{
					if (controller.InUse)
						leapController.InUse = false;

					// If the controller in the same hand as the leap controller is in use, make sure the leap controller is not in use.
					// Conversely, if the controller is not in use make sure the leap controller is in use
					controller.InUseChangedEvent += inUse => leapController.InUse = !inUse;
				}
			};
		}
	}
}