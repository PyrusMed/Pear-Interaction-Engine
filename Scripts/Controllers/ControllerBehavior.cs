using System;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// Based class for script that need to use a controller
	/// </summary>
	/// <typeparam name="T">Type of controller</typeparam>
	public class ControllerBehavior<T> : ControllerBehaviorBase, IControllerBehavior<T> where T : Controller
    {
		// The controller
		[SerializeField]
        private T _controller;

		/// <summary>
		/// The controller
		/// </summary>
		public T Controller
		{
			get { return _controller; }
		}

		void Start()
		{
			// If the controller was not set, try to set it automatically
			_controller = _controller ?? GetComponent<T>();
		}
    }
}