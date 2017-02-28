using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Based class for script that need to use a controller
	/// </summary>
	/// <typeparam name="T">Type of controller</typeparam>
	[RequireComponent(typeof(Controller))]
	public class ControllerBehavior<T> : MonoBehaviour where T : Controller
    {
        private T _controller;
        protected T Controller
        {
            get
            {
                return _controller ?? (_controller = GetComponent<T>());
            }
        }
    }
}