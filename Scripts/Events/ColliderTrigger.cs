using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Properties;
using UnityEngine;
using Pear.InteractionEngine.Controllers;

namespace Pear.InteractionEngine.Events
{
	public class ColliderTrigger : ControllerBehavior<Controller>, IEvent<Collider>
	{
		// The event
		public Property<Collider> Event { get; set; }

		private void OnTriggerEnter(Collider collider)
		{
			Event.Value = collider;
		}
		private void OnTriggerExit(Collider collider)
		{
			Event.Value = null;
		}
	}
}
