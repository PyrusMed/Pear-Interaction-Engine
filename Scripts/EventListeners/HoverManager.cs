using System;
using System.Collections;
using System.Collections.Generic;
using Pear.InteractionEngine.Events;
using UnityEngine;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.EventListeners
{
	public class HoverManager : MonoBehaviour, IEventListener<RaycastHit?>
	{
		private const string LOG_TAG = "[HoverManager]";

		[Tooltip("Manages selection")]
		public SelectionManager SelectionManager;

		[Tooltip("The layer to interact with")]
		public LayerMask InteractableLayers;

		[Tooltip("The emmission strength")]
		[Range(1f, 100f)]
		public float EmissionStrength = 1f;

		[Tooltip("Pulses per second")]
		[Range(1f, 100f)]
		public float PulsesPerSecond = 1;

		/// <summary>
		/// The hovered object, if any
		/// </summary>
		public GameObject HoveredObject
		{
			get;
			private set;
		}

		/// <summary>
		/// Tells whether an object is being hovered over
		/// </summary>
		public bool IsHoveringOverObject { get { return HoveredObject != null; } }

		private void Start()
		{
			// When a new object is selected
			// make sure we stop showing the hover effect
			SelectionManager.SelectEvent += selectedObject => UpdateHoverEffect(selectedObject, showEffect: false);

			// When an object is deselected 
			// make sure we start showing the hover effect
			// on the hovered object
			SelectionManager.DeselectEvent += deselectedObject => UpdateHoverEffect(HoveredObject, showEffect: CanHover(HoveredObject));
		}

		public void ValueChanged(EventArgs<RaycastHit?> args)
		{
			// If we're dealing with the same gameobject, return
			if(args.NewValue.HasValue &&
				args.OldValue.HasValue &&
				args.NewValue.Value.transform.gameObject == args.OldValue.Value.transform.gameObject)
			{
				return;
			}

			// If there's an old hit remove it
			if (IsValid(args.OldValue))
			{
				GameObject objectToRemoveHoverFrom = args.OldValue.Value.transform.gameObject;

				UpdateHoverEffect(objectToRemoveHoverFrom, showEffect: false);

				HoveredObject = null;
			}

			// If there's a new hit update the active object
			if (IsValid(args.NewValue))
			{
				GameObject objectToHover = args.NewValue.Value.transform.gameObject;

				UpdateHoverEffect(objectToHover, showEffect: CanHover(objectToHover));

				HoveredObject = objectToHover;
			}
		}

		/// <summary>
		/// Checks whether the given object can be hovered over
		/// </summary>
		/// <param name="hit">The raycast hit</param>
		/// <returns>True if object can be hovered over. False otherwise</returns>
		private bool IsValid(RaycastHit? hit)
		{
			return hit.HasValue &&
				InteractableLayers == (InteractableLayers | (1 << hit.Value.transform.gameObject.layer));
		}

		/// <summary>
		/// Tells whether we can hover over this object
		/// </summary>
		/// <param name="objToCheck">Object to check</param>
		/// <returns>True if we can hover. False otherwise.</returns>
		private bool CanHover(GameObject objToCheck)
		{
			return !SelectionManager.IsSelected(objToCheck) && (SelectionManager.MultipleSelection || !SelectionManager.HasSelectedObject);
		}

		/// <summary>
		/// Updates the hover effect on the given object
		/// </summary>
		/// <param name="objToUpdate">The obj to update</param>
		/// <param name="showEffect">Should we show the effect or hide it?</param>
		private void UpdateHoverEffect(GameObject objToUpdate, bool showEffect)
		{
			if (objToUpdate == null)
				return;

			EmissionHighlight hoverHighlight = objToUpdate.transform.GetOrAddComponent<EmissionHighlight>(onAdd: newHoverHighlight =>
			{
				newHoverHighlight.EmissionStrength = EmissionStrength;
				newHoverHighlight.PulsesPerSecond = PulsesPerSecond;
			});

			hoverHighlight.Highlight = showEffect;
		}
	}
}
