using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Events;
using UnityEngine;
using Pear.InteractionEngine.Utils;
using System.Linq;

namespace Pear.InteractionEngine.EventListeners
{
	public class SelectionManager : MonoBehaviour, IEventListener<bool>
	{
		private const string LOG_TAG = "[SelectionManager]";

		[Tooltip("Manages hover events")]
		public HoverManager HoverManager;

		[Tooltip("Outline material")]
		public Material OutlineMaterialTemplate;

		[Tooltip("The value of the outline width when the outline is showing")]
		public float OutlineWidth = 0.005f;

		[Tooltip("Can multiple objects be selected at once?")]
		public bool MultipleSelection = false;

		// Selection events
		public event Action<GameObject> DeselectEvent;
		public event Action<GameObject> SelectEvent;

		/// <summary>
		/// The currently selected object
		/// </summary>
		private List<SelectedInfo> _selectedObjects = new List<SelectedInfo>();

		/// <summary>
		/// Tells whether there is a selected object
		/// </summary>
		public bool HasSelectedObject { get { return _selectedObjects.Count > 0; } }

		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue && HoverManager.IsHoveringOverObject)
			{
				// If the hovered object is selected, deselect it
				SelectedInfo hoveredSelectionInfo = _selectedObjects.Find(selectedInfo => selectedInfo.SelectedObject == HoverManager.HoveredObject);
				if (hoveredSelectionInfo != null)
					Deselect(hoveredSelectionInfo);
				else
					Select(HoverManager.HoveredObject, args.Source);
			}
		}

		/// <summary>
		/// Tells whether the given object is selected
		/// </summary>
		/// <param name="objectToCheck">the object to check</param>
		/// <returns>True if object is selected. False otherwise.</returns>
		public bool IsSelected(GameObject objectToCheck)
		{
			return _selectedObjects.Any(selected => selected.SelectedObject == objectToCheck);
		}

		/// <summary>
		/// Select the object
		/// </summary>
		/// <param name="objectToSelect">Object to select</param>
		/// <param name="source">Source controller</param>
		private void Select(GameObject objectToSelect, Controller source)
		{
			//Debug.Log(String.Format("{0} Selecting {1}", LOG_TAG, objectToSelect.name));

			// If there's not multiple selection deselect everything
			if (!MultipleSelection)
				DeselectAll();

			_selectedObjects.Add(new SelectedInfo
			{
				SelectedObject = objectToSelect,
				Source = source,
			});

			UpdateOutline(objectToSelect, showOutline: true);
			source.AddActives(objectToSelect);

			if (SelectEvent != null)
				SelectEvent(objectToSelect);
		}

		/// <summary>
		/// Deselect all selected items
		/// </summary>
		private void DeselectAll()
		{
			SelectedInfo[] selectedItems = _selectedObjects.ToArray();
			foreach (SelectedInfo selected in selectedItems)
				Deselect(selected);
		}

		/// <summary>
		/// Deselect the given object
		/// </summary>
		private void Deselect(SelectedInfo deselectionInfo)
		{
			// Remove old selection, if any
			if (deselectionInfo != null)
			{
				//Debug.Log(String.Format("{0} deselecting {1}", LOG_TAG, deselectionInfo.SelectedObject.name));

				_selectedObjects.Remove(deselectionInfo);

				UpdateOutline(deselectionInfo.SelectedObject, showOutline: false);
				deselectionInfo.Source.RemoveActives(deselectionInfo.SelectedObject);

				if (DeselectEvent != null)
					DeselectEvent(deselectionInfo.SelectedObject);
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
			});

			outline.ShowOutline = showOutline;
		}

		private class SelectedInfo
		{
			public GameObject SelectedObject;
			public Controller Source;
		}
	}
}
