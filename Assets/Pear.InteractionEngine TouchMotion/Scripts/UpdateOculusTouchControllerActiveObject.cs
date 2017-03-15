using Pear.InteractionEngine.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a link between the active object of the oculus touch controllers and the active object of other controllers
/// This make sure that the touch controllers have an active object to work on
/// </summary>
public class UpdateOculusTouchControllerActiveObject : MonoBehaviour {

	[Tooltip("The left Oculus touch controller")]
	public OculusTouchController LeftController;

	[Tooltip("The right Oculus touch controller")]
	public OculusTouchController RightController;

	/// <summary>
	/// We want to Oculus Touch's active object to be the active object on the controller in the opposite hand.
	/// For example, if the left leap motion hand is holding an object we want the right touch controller to control that object.
	/// Reasoning: If you're holding a controller in one hand you need the other hand to do the selection.
	/// </summary>
	void Awake()
	{
		EnsureTouchControllerLocationIsCorrect(LeftController);
		EnsureTouchControllerLocationIsCorrect(RightController);

		// Attempt to create a link between the new controller's active object and one of the touch controllers
		ControllerManager.Instance.ControllerAddedEvent += AttemptToCreateActiveObjectLink;
	}

	/// <summary>
	/// Make sure the controller's location is correct
	/// </summary>
	/// <param name="controller">controller to check</param>
	private void EnsureTouchControllerLocationIsCorrect(OculusTouchController controller)
	{
		if (controller.Location != ControllerLocation.LeftHand && controller.Location != ControllerLocation.RightHand)
			throw new MissingReferenceException("Oculus touch controllers should be in either the left or right hand");
	}

	/// <summary>
	/// If the given controller is in the opposite hand set the Touch controller's active object
	/// to the opposite-hand-controller's active object when it changes
	/// </summary>
	/// <param name="controller">controller to potentially link</param>
	private void AttemptToCreateActiveObjectLink(Controller controller)
	{
		Debug.Log("Controller added: " + controller.name);

		// Ignore touch controllers
		if (controller is OculusTouchController)
			return;

		// Get the oculus touch controller
		// We want the controller in the opposite hand
		OculusTouchController touchController = null;
		if (controller.Location == ControllerLocation.LeftHand)
			touchController = RightController;
		if (controller.Location == ControllerLocation.RightHand)
			touchController = LeftController;

		// If this controller is in one of the hands link it's active object
		if (touchController != null)
		{
			Debug.Log(string.Format("Listening for active object changes on {0} for {1}", controller.name, touchController.name));
			controller.ActiveObjectChangedEvent += (oldActive, newActive) => { touchController.ActiveObject = newActive; };
		}
	}
}
