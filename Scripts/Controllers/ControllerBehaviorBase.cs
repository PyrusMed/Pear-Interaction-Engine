using UnityEngine;

namespace Pear.InteractionEngine.Controllers
{
	/// <summary>
	/// This interface exists to give Events flexibility. We need to expose the controller
	/// to event handlers via the Interaction class, but we don't want to require controllers to implement ControllerBehavior<T>.
	/// </summary>
	/// <typeparam name="T">Type of controller</typeparam>
	public interface IControllerBehavior<T> where T : Controller
	{
		T Controller { get; }
	}

	/// <summary>
	/// This class only exists so we can create an editor for the templated type ControllerBehavior<T>.
	/// This class should not be inherited by any other class.
	/// </summary>
	public class ControllerBehaviorBase : MonoBehaviour { }
}
