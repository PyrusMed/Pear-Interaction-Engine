using Leap;
using Leap.Unity;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Pear.InteractionEngine.Interactions.Events
{
	/// <summary>
	/// Detects when the leap hand starts grabbing an object and lets the object know
	/// </summary>
	public class HandGrab : ControllerBehavior<LeapMotionController>, IGameObjectPropertyEvent<IHandModel>
	{
		// Detects whether the hand is pinching
		private PinchDetector _pinchDetector;

		// The last object that was hovered over
		private GameObject _lastHovered;

		// Registered proeprties
		private List<GameObjectProperty<IHandModel>> _properties = new List<GameObjectProperty<IHandModel>>();

		// Use this for initialization
		void Start()
		{
			_pinchDetector = Controller.Hand.gameObject.GetComponentInChildren<PinchDetector>();

			// Detects when objects are in close proximity
			ProximityDetector proximityDetector = Controller.Hand.gameObject.GetComponentInChildren<ProximityDetector>();

			// Save the last RTS we hovered over
			proximityDetector.OnProximity.AddListener(hovered => _lastHovered = hovered);

			// Detect when we stop hovering
			proximityDetector.OnDeactivate.AddListener(() => _lastHovered = null);
		}

		void Update()
		{
			// If we started pinching and we're hoving over an object...grab it
			if (_pinchDetector.DidStartPinch && _lastHovered != null)
			{
				_properties.Where(p => p.Owner == _lastHovered)
					.ToList()
					.ForEach(p => p.Value = Controller.Hand);
			}
			// Otherwise, if we just stopped pinching let go of all objects
			else if (_pinchDetector.DidEndPinch)
			{
				_properties.ForEach(p => p.Value = null);
			}
		}

		public void RegisterProperty(GameObjectProperty<IHandModel> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<IHandModel> property)
		{
			_properties.Remove(property);
		}
	}
}
