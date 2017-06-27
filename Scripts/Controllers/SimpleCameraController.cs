using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Camera controller that changes active object based on gaze
	/// </summary>
	[RequireComponent(typeof(PhysicsRaycast))]
	public class SimpleCameraController : Controller
	{
		public override void Start()
		{
			base.Start();

			// Change the active object based on gaze
			PhysicsRaycast raycast = GetComponent<PhysicsRaycast>();
			if (raycast != null)
			{
				raycast.Event.ValueChangeEvent += (oldValue, newValue) =>
				{
					if (newValue != null)
						ActiveObject = newValue.Value.transform.gameObject;
					else
						ActiveObject = null;
				};
			}
		}
	}
}
