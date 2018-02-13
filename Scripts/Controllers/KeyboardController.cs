
namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Controller for the keyboard
	/// </summary>
	public class KeyboardController : Controller
	{
		// Camera controller
		public Controller CameraController;

		protected override void Start()
		{
			base.Start();

			// Set the keyboard controller's active object to whatever the given controller's active object is
			if(CameraController != null)
			{
				CameraController.PostActiveObjectsChangedEvent += (oldActiveObjects, newActiveObjects) =>
				{
					ActiveObjects = newActiveObjects;
				};
			}
		}
	}
}