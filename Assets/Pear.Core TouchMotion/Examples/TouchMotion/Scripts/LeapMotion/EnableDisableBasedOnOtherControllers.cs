using Pear.Core.Controllers;
using UnityEngine;

/// <summary>
/// Enables or disables the leap motion controllers based on other controllers in the same hand
/// </summary>
public class EnableDisableBasedOnOtherControllers : MonoBehaviour {

	[Tooltip("Left controller")]
	public LeapMotionController Left;

	[Tooltip("Right controller")]
	public LeapMotionController Right;

	// Use this for initialization
	void Start () {
		// Loop over each controller and hook up events to non-leap motion controllers
		// that occupy the same hands
		foreach (Controller controller in ControllerManager.Instance.Controllers)
		{
			if (controller is LeapMotionController)
				continue;

			// Get the correct leap controller
			LeapMotionController leapController = null;
			if (controller.Location == Left.Location)
				leapController = Left;
			else if (controller.Location == Right.Location)
				leapController = Right;

			// If there's a leap controller in the same location as another controller
			// make sure the leap controller is not showing when the other controller is enabled
			if(leapController != null)
			{
				if (controller.InUse)
					leapController.InUse = false;

				// When the other controller is enabled, disable the leap motion
				controller.OnStartUsing.AddListener((c) => leapController.InUse = false);

				// When the other controller is disabled, enable the leap motion
				controller.OnStopUsing.AddListener((c) => leapController.InUse = true);
			}
		}
	}
}
