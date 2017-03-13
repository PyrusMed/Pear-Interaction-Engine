using Pear.InteractionEngine.Properties;
using System.Collections.Generic;
using UnityEngine;
using Pear.InteractionEngine.Controllers;
using System.Linq;

namespace Pear.InteractionEngine.Events
{
	public class TouchControl : ControllerBehavior<OculusTouchController>, IGameObjectPropertyEvent<bool>
	{
		[Tooltip("The touch control who's change we're listening for")]
		public OVRInput.Touch Touch;

		// Registered properties
		private List<GameObjectProperty<bool>> _properties = new List<GameObjectProperty<bool>>();

		void Update()
		{
			bool isPressed = OVRInput.Get(Touch, Controller.OVRController);
			_properties.Where(p => p.Owner == Controller.ActiveObject).ToList().ForEach(p => p.Value = isPressed);
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