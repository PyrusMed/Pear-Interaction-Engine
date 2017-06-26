using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	public class KeyboardController : Controller
	{
		public SimpleCameraController CameraController;

		public override void Start()
		{
			base.Start();

			// Set the keyboard controller's active object to whatever the camera is looking at
			if(CameraController != null)
			{
				CameraController.ActiveObjectChangedEvent += (oldActiveObject, newActiveObject) =>
				{
					ActiveObject = newActiveObject;
				};
			}
		}
	}
}