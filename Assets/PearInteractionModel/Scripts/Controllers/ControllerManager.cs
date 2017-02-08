using PearMed.Utils;
using System.Collections.Generic;

namespace PearMed.Controllers
{
    /// <summary>
    /// Singleton responsible for managing controllers
    /// </summary>
    public class ControllerManager : Singleton<ControllerManager>
    {

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
        }
    }
}