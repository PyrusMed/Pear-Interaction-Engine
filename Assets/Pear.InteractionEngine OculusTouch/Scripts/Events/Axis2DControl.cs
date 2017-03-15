using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	public class Axis2DControl : ControllerBehavior<OculusTouchController>, IGameObjectPropertyEvent<Vector2>
	{
		[Tooltip("The control who's changes we're listening for")]
		public OVRInput.Axis2D Control;

		// Registered properties
		private List<GameObjectProperty<Vector2>> _properties = new List<GameObjectProperty<Vector2>>();

		void Update()
		{
			Vector2 newValue = OVRInput.Get(Control, Controller.OVRController);
			_properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = newValue);
		}

		public void RegisterProperty(GameObjectProperty<Vector2> property)
		{
			_properties.Add(property);
		}

		public void UnregisterProperty(GameObjectProperty<Vector2> property)
		{
			_properties.Remove(property);
		}
	}
}
