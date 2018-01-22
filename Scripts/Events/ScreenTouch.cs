using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Event for touching the screen
	/// </summary>
	public class ScreenTouch : ControllerBehavior<Controller>, IEvent<bool>
	{

		[Tooltip("Number of touches needed to fire the event")]
		[Range(1, 4)]
		public int NumberOfTouches = 1;

		// Stores the event value that's handled by IEventListener classes
		public Property<bool> Event { get; set; }

		void Update()
		{
			Event.Value = Input.touchCount == NumberOfTouches;
		}
	}
}
