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

		// Stores information about the event
		public Property<RaycastHit?> Event { get; set; }

		void Update()
        {
			// Send a ray out to see if we're looking at anything
			{
				// If we hit something update the event property
				RaycastHit hitInfo;
				Vector3 relativeDirection = Controller.transform.TransformVector(Direction);
				if (Physics.Raycast(Camera.main.transform.position, relativeDirection, out hitInfo, 1000))
					Event.Value = hitInfo;
				// Otherwise, set the event property to null
				else
					Event.Value = null;
			}
        }
	}
}