using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Event for pressing a key on the keyboard
	/// </summary>
	public class ControllerRotation : ControllerBehavior<KeyboardController>, IEvent<Vector3>
	{
		[Tooltip("Key that fires the event")]
		public IEvent<bool> Trigger;

		// Stores the event value that's handled by IEventListener classes
		public Property<Vector3> Event { get; set; }

		private Vector3 _triggerRotation;

		void Start()
		{
			if(Trigger != null)
			{
				Trigger.Event.ValueChangeEvent += (oldValue, newValue) => _triggerRotation = transform.eulerAngles;
			}
		}

		void Update()
		{
			if(Trigger.Event.Value)
			{

			}
		}
	}
}
