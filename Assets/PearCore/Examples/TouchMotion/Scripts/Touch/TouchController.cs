using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pear.Core.Controllers;
using Pear.Core.Interactables;

/// <summary>
/// Defines how the Touch controller interacts with objects and the environment
/// </summary>
public class TouchController : Controller
{
	// Hand element that will tell us if the controller is active or not
	private GameObject _handRenderElement;

	public OVRInput.Controller OVRController
	{
		get;
		private set;
	}

	// Hook up events
	void Start()
	{
		OVRController = Location == ControllerLocation.LeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;

		GetHandRenderElement();
		AttachRotationAnZoomEvents();
		AttachGrabEvents();
    }

	void Update()
	{
		if (_handRenderElement != null && _handRenderElement.activeInHierarchy != InUse)
			InUse = _handRenderElement.activeInHierarchy;
	}

	/// <summary>
	/// Wait for the OVRAvatar assets to finish loading, then get the hand render element
	/// </summary>
	private void GetHandRenderElement()
	{
		// Get the hand element associated with this controller
		string nameOfHandElement = Location == ControllerLocation.LeftHand ? "hand_left" : "hand_right";
		Transform handElement = transform.parent.Find(nameOfHandElement);

		// When the assets are done loading the hand element will have a single child
		// That's the hand render element
		OvrAvatar avatar = FindObjectOfType<OvrAvatar>();
		avatar.AssetsDoneLoading.AddListener(() =>
		{
			_handRenderElement = handElement.GetChild(0).gameObject;
        });
	}

	private void AttachRotationAnZoomEvents()
	{
		TouchRotation touchRotation = GetComponent<TouchRotation>();
		TouchZoom touchZoom = GetComponent<TouchZoom>();
		if (touchZoom == null)
			Debug.Log("Holy shitsnacks!");

        // When a object is added hook up events
        foreach (InteractableObject interactable in InteractableObjectManager.Instance.AllObjects)
        {
			// When we start moving an object make sure we can rotate and zoom it
			interactable.Moving.OnStart.AddListener((e) =>
			{
				ActiveObject = interactable;
			});

			// When we stop moving an object default to rotating the whole model
			interactable.Moving.OnEnd.AddListener((e) =>
			{
				ActiveObject = null;
			});
		}

		// Listen for rotation events
		{
			// When we  start rotating add the controller to the object's events
			touchRotation.OnStart.AddListener((interactable) =>
			{
				interactable.Rotating.Add(this);
			});

			// When we stop rotating remove the controller from the object's events
			touchRotation.OnEnd.AddListener((interactable) =>
			{
				interactable.Rotating.Remove(this);
			});
		}

		// Listen for zooming events
		{
			// When we start zooming add the controller to the object's events
			touchZoom.OnStart.AddListener((interactable) =>
			{
				interactable.Resizing.Add(this);
			});

			// When we stop zooming remove the controller from the object's events
			touchZoom.OnEnd.AddListener((interactable) =>
			{
				interactable.Resizing.Remove(this);
			});
		}
	}

	/// <summary>
	/// Attach grab events so interactable objects know when they've been grabbed
	/// </summary>
	private void AttachGrabEvents()
	{
		TouchGrab grab = GetComponent<TouchGrab>();
		grab.OnGrab.AddListener((interactable) => interactable.Moving.Add(this));
		grab.OnRelease.AddListener((interactable) => interactable.Moving.Remove(this));
	}
}