using Pear.Core.Controllers;
using Pear.Core.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Grab the entire model with the touch controller
/// </summary>
public class TouchGrab : ControllerBehavior<TouchController> {

	[Tooltip("Fired when the model is grabbed")]
	public InteractableObjectEvent OnGrab = new InteractableObjectEvent();

	[Tooltip("Fired when the model is released")]
	public InteractableObjectEvent OnRelease = new InteractableObjectEvent();

	// Start grabbing the model after this threshold is reached
	private const float GrabThreshold = 0.9f;

	// True if grabbing. False otherwise.
	private bool _isGrabbing = false;

	private Transform _originalParent;

	private TouchController _touchController;

	void Start()
	{
		_touchController = GetComponent<TouchController>();
	}

	// Update is called once per frame
	void Update () {
		// When the user pressed the grab button grab the model
		if (!_isGrabbing && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _touchController.Controller) > GrabThreshold)
		{
			_isGrabbing = true;
			GrabWholeModel();
        }

		// When the user releases the grab button release the model
		if (_isGrabbing && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, _touchController.Controller) < GrabThreshold)
		{
			_isGrabbing = false;
			ReleaseWholeModel();
		}
	}

	/// <summary>
	/// Grab the whole model if it's loaded
	/// </summary>
	private void GrabWholeModel()
	{
		if(Controller.ActiveObject != null)
		{
			// Make the model's anchor element a child of the controller so it moves with the controller
			InteractableObject model = Controller.ActiveObject;
			_originalParent = model.AnchorElement.transform.parent;
            model.AnchorElement.transform.SetParent(transform, true);
			OnGrab.Invoke(model);
		}
	}

	/// <summary>
	/// Release  the whole model if it's loaded
	/// </summary>
	private void ReleaseWholeModel()
	{
		if (Controller.ActiveObject != null)
		{
			// Reparent the model back to it's original parent
			InteractableObject model = Controller.ActiveObject;
			model.AnchorElement.transform.SetParent(_originalParent, true);
			OnRelease.Invoke(model);
		}
	}
}
