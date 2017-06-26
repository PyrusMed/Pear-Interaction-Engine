using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	public class KeyPress : ControllerBehavior<KeyboardController>, IEvent<bool>
	{

		[Tooltip("Key that fires event")]
		public KeyCode Key = KeyCode.Alpha0;

		public Property<bool> Event { get; set; }

		void Update()
		{
			Event.Value = Input.GetKey(Key);
		}
	}
}
