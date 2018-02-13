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

		[Tooltip("The layer to interact with")]
		public LayerMask InteractableLayers;

		[Tooltip("The emmission strength")]
		[Range(1f, 100f)]
		public float EmissionStrength = 1f;

		[Tooltip("Pulses per second")]
		[Range(1f, 100f)]
		public float PulsesPerSecond = 1;

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

				UpdateHoverOutline(objectToRemoveHoverFrom, showOutline: false);

				args.Source.RemoveActives(objectToRemoveHoverFrom);
			}

			// If there's a new hit update the active object
			if (IsValid(args.NewValue))
			{
				GameObject objectToHover = args.NewValue.Value.transform.gameObject;

				UpdateHoverOutline(objectToHover, showOutline: true);

				args.Source.AddActives(objectToHover);
			}
		}

		private bool IsValid(RaycastHit? hit)
		{
			return hit.HasValue &&
				InteractableLayers == (InteractableLayers | (1 << hit.Value.transform.gameObject.layer));
		}

		private void UpdateHoverOutline(GameObject obj, bool showOutline)
		{
			EmissionHighlight hoverOutline = obj.transform.GetOrAddComponent<EmissionHighlight>(onAdd: newHoverOutline =>
			{
				newHoverOutline.EmissionStrength = EmissionStrength;
				newHoverOutline.PulsesPerSecond = PulsesPerSecond;
			});

			hoverOutline.Highlight = showOutline;
		}
	}
}
