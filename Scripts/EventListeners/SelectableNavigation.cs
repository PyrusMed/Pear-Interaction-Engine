using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Events;
using System;
using UnityEngine.UI;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Navigates across selectables based on input
	/// </summary>
	public class SelectableNavigation : MonoBehaviour, IEventListener<Vector2>
	{
		[Tooltip("Moves when the inputted value is above this threshold")]
		public float MoveThreshold = 0.5f;

		[Tooltip("Waits this long before move can occur again")]
		public float MoveDelay = 0.5f;

		// Has the move threshold expired?
		private bool _canMove = true;

		// The currently selected object
		private Selectable _selected;

		// Fired when selection changes
		public Action<Selectable> SelectedChangedEvent;

		/// <summary>
		/// Select the given object
		/// </summary>
		/// <param name="selectable"></param>
		public void Select(Selectable selectable)
		{
			if (selectable != null)
			{
				selectable.Select();

				if (SelectedChangedEvent != null)
					SelectedChangedEvent(selectable);

				_selected = selectable;
			}

			StartCoroutine(WaitToMove(MoveDelay));
		}

		/// <summary>
		/// Select objects based on input
		/// </summary>
		/// <param name="args"></param>
		public void ValueChanged(EventArgs<Vector2> args)
		{
			if (!_canMove || _selected == null)
				return;

			Selectable next = null;
			float absX = Mathf.Abs(args.NewValue.x);
			float absY = Mathf.Abs(args.NewValue.y);

			if (absX > absY && absX > MoveThreshold)
				next = args.NewValue.x > 0 ? _selected.FindSelectableOnRight() : _selected.FindSelectableOnLeft();
			else if (absY > absX && absY > MoveThreshold)
				next = args.NewValue.y > 0 ? _selected.FindSelectableOnUp() : _selected.FindSelectableOnDown();

			if (next != null)
				Select(next);
		}

		/// <summary>
		/// Waits a given amount of time before we can move
		/// </summary>
		/// <param name="seconds">Number of seconds to wait</param>
		private IEnumerator WaitToMove(float seconds)
		{
			_canMove = false;
			yield return new WaitForSeconds(seconds);
			_canMove = true;
		}
	}
}
