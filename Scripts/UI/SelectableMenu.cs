using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.Models;
using System;
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
		public SelectableNavigation SliderNavigation;

		[Tooltip("Called when the menu opens")]
		public ControllerEvent OnOpen = new ControllerEvent();

		[Tooltip("Called when the menu closes")]
		public ControllerEvent OnClose = new ControllerEvent();

		// The controller we're currently open around
		private Controller _activeController;

		private void Awake()
		{
			// When selection changes set the active objects
			SliderNavigation.SelectedChangedEvent += (selected) =>
			{
				if (_activeController != null)
					_activeController.SetActive(gameObject, selected.gameObject);
			};
		}

		/// <summary>
		/// Place the menu around the given controller
		/// </summary>
		/// <param name="go"></param>
		public void Open(Controller controller)
		{
			gameObject.SetActive(true);

			_activeController = controller;
			controller.SetActive(gameObject);

			// Select the default element
			SliderNavigation.Select(DefaultSelected);

			OnOpen.Invoke(_activeController);
		}

		/// <summary>
		/// Close the menu
		/// </summary>
		/// <param name="controller"></param>
		public void Close()
		{
			Debug.Log("Closing selectable menu: " + name);
			gameObject.SetActive(false);

			if(_activeController != null)
				_activeController.RemoveActives(gameObject);

			OnClose.Invoke(_activeController);
		}
	}
}
