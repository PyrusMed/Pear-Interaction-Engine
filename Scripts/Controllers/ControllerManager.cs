﻿using Pear.InteractionEngine.Utils;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Controllers
{
    /// <summary>
    /// Singleton responsible for managing controllers
    /// </summary>
    public class ControllerManager : Singleton<ControllerManager>
    {
		// Event for when a controller is added
		public delegate void ControllerAddedHandler(Controller controller);
		public event ControllerAddedHandler ControllerAddedEvent;

        // List of all loaded controllers
        private List<Controller> _controllers = new List<Controller>();

        /// <summary>
        /// Array of controllers that are loaded
        /// </summary>
        public Controller[] Controllers
        {
            get { return _controllers.ToArray(); }
        }

        /// <summary>
        /// Register a controller that's been loaded
        /// </summary>
        /// <param name="controller">Controller that's been loaded</param>
        public void RegisterController(Controller controller)
        {
            _controllers.Add(controller);

			// Let listeners know
			if (ControllerAddedEvent != null)
				ControllerAddedEvent(controller);
		}
    }
}