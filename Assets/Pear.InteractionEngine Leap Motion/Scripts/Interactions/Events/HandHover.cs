using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Leap.Unity;
using System.Linq;

namespace Pear.InteractionEngine.Interactions.Events
{
	public class HandHover : ControllerBehavior<LeapMotionController>, IGameObjectPropertyEvent<bool>
	{
		// Registered proeprties
		private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

		void Awake()
		{
			ProximityDetector proximityDetector = Controller.gameObject.GetComponentInChildren<ProximityDetector>();

			// When the hand starts hovering over the object
			// let the object know
			proximityDetector.OnProximity.AddListener(hoverObj => _properties.ForEach(p => p.Value = p.Owner == hoverObj));

			// When the hand stops hovering over the object
			// let the object know
			proximityDetector.OnDeactivate.AddListener(() => _properties.ForEach(p => p.Value = false));
		}

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			_properties.Remove(property);
		}

		
	}
}
