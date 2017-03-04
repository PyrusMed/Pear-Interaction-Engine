using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Based class for script that need to use a controller
	/// </summary>
	/// <typeparam name="T">Type of controller</typeparam>
	public class ControllerBehavior<T> : MonoBehaviour where T : Controller
    {
		// The controller
        public T Controller;

		void Start()
		{
			// If the controller was not set, try to set it automatically
			Controller = Controller ?? GetComponent<T>();
		}
    }
}