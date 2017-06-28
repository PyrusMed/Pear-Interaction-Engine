using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Event for pressing a key on the keyboard
	/// </summary>
	public class KeyPress : ControllerBehavior<KeyboardController>, IEvent<bool>
	{

		[Tooltip("Key that fires the event")]
		public KeyCode Key = KeyCode.Alpha0;

		// Stores the event value that's handled by IEventListener classes
		public Property<bool> Event { get; set; }

		void Update()
		{
			// Update the event's value with the state of the key
			Event.Value = Input.GetKey(Key);
		}
	}
}
