using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// A menu that opens around a controller
	/// and uses selectable navigation
	/// </summary>
	public class SelectableMenu : MonoBehaviour
	{
		[Tooltip("The default selected object")]
		public Selectable DefaultSelected;

		[Tooltip("Offset from the controller")]
		public Vector3 PositionOffset;

		[Tooltip("Local rotation around the parent controller")]
		public Vector3 RotationOffset;

		[Tooltip("The slider selector")]
		public SelectableNavigation SliderSelector;

		/// <summary>
		/// Place the menu around the given controller
		/// </summary>
		/// <param name="go"></param>
		public void OpenAround(Controller controller)
		{
			Debug.Log("Opening density menu above " + controller.name);

			gameObject.SetActive(true);

			controller.ActiveObject = gameObject;

			// Anchor to the given controller
			transform.SetParent(controller.transform, false);

			// Place the menu above the controller
			transform.localPosition = PositionOffset;
			transform.localEulerAngles = RotationOffset;

			// Select the default element
			SliderSelector.Select(controller, DefaultSelected);
		}

		/// <summary>
		/// Close the menu
		/// </summary>
		/// <param name="controller"></param>
		public void Close(Controller controller)
		{
			Debug.Log("Closing density menu");

			controller.ActiveObject = ModelLoader.Instance.LoadedModel;

			gameObject.SetActive(false);
		}
	}
}
