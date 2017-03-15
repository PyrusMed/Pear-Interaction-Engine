using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	public class Axis1DControl : ControllerBehavior<OculusTouchController>, IGameObjectPropertyEvent<float>
	{
		[Tooltip("The control who's changes we're listening for")]
		public OVRInput.Axis1D Control;

		// Registered properties
		private List<GameObjectProperty<float>> _properties = new List<GameObjectProperty<float>>();

		void Update()
		{
			float newValue = OVRInput.Get(Control, Controller.OVRController);
			_properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = newValue);
		}

		public void RegisterProperty(GameObjectProperty<float> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<float> property)
		{
			_properties.Remove(property);
		}
	}
}