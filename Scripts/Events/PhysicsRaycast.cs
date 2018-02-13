using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Raycasts in the specified direction, from the controller, and emits hit info
	/// </summary>
	public class PhysicsRaycast : ControllerBehavior<Controller>, IEvent<RaycastHit?>
	{
		[Tooltip("Direction to raycast")]
		public Vector3 Direction = new Vector3(0, 0, 1);

		// Stores the event value that's handled by IEventListener classes
		public Property<RaycastHit?> Event { get; set; }

		void Update()
        {
			// Send a ray out to see if we're looking at anything
			{
				// If we hit something update the event property
				RaycastHit hitInfo;
				if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1000))
				{
					Event.Value = hitInfo;
				}
				// Otherwise, set the event property to null
				else
				{
					Event.Value = null;
				}
			}
        }
	}
}