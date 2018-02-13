using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Events;
using UnityEngine;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.EventListeners
{
	public class SelectionManager : Singleton<SelectionManager>, IEventListener<bool>
	{
		private const string LOG_TAG = "[SelectionManager]";

		[Tooltip("Outline material")]
		public Material OutlineMaterialTemplate;

		[Tooltip("The value of the outline width when the outline is showing")]
		public float HoveredOutlineValue = 0.2f;

		// Selection events
		public event Action<GameObject> DeselectEvent;
		public event Action<GameObject> SelectEvent;

		// The source of the currently selected object
		private Controller _selectedObjectSource;

		/// <summary>
		/// The currently selected object
		/// </summary>
		public GameObject SelectedObject { get; private set; }

		/// <summary>
		/// Tells whether there is a selected object
		/// </summary>
		public bool HasSelectedObject { get { return SelectedObject != null; } }

		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue && HoverManager.Instance.IsHoveringOverObject)
			{
				if (HoverManager.Instance.HoveredObject == SelectedObject)
					Deselect(SelectedObject);
				else
					Select(HoverManager.Instance.HoveredObject, args.Source);
			}
		}

		/// <summary>
		/// Select the object
		/// </summary>
		/// <param name="objectToSelect">Object to select</param>
		/// <param name="source">Source controller</param>
		private void Select(GameObject objectToSelect, Controller source)
		{
			Deselect(SelectedObject);

			SelectedObject = objectToSelect;
			_selectedObjectSource = source;

			UpdateOutline(SelectedObject, showOutline: true);
			_selectedObjectSource.SetActive(SelectedObject);

			if (SelectEvent != null)
				SelectEvent(SelectedObject);
		}

		/// <summary>
		/// Deselect the given object
		/// </summary>
		/// <param name="objectToDeselect">Object to deselect</param>
		private void Deselect(GameObject objectToDeselect)
		{
			// Remove old selection, if any
			if (SelectedObject != null && _selectedObjectSource != null)
			{
				GameObject oldSelected = SelectedObject;
				Controller oldSelectedSource = _selectedObjectSource;

				SelectedObject = null;
				_selectedObjectSource = null;

				UpdateOutline(oldSelected, showOutline: false);
				oldSelectedSource.RemoveActives(oldSelected);

				if (DeselectEvent != null)
					DeselectEvent(oldSelected);
			}
		}

		/// <summary>
		/// Update the outline around the give object
		/// </summary>
		/// <param name="obj">object to update</param>
		/// <param name="showOutline">Tells whether we should show the outline or not</param>
		private void UpdateOutline(GameObject obj, bool showOutline)
		{
			Outline outline = obj.transform.GetOrAddComponent<Outline>(onAdd: newOutline =>
			{
				newOutline.OutlineMaterialTemplate = OutlineMaterialTemplate;
				newOutline.HoveredOutlineValue = HoveredOutlineValue;
			});

			outline.ShowOutline = showOutline;
		}
	}
}
