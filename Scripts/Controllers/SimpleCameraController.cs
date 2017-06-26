using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	[RequireComponent(typeof(PhysicsRaycast))]
	public class SimpleCameraController : Controller
	{
		public override void Start()
		{
			base.Start();

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
