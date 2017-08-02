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
		/// <param name="controller"></param>
		/// <param name="selectable"></param>
		public void Select(Controller controller, Selectable selectable)
		{
			if (selectable != _selected && selectable != null)
			{
				_selected = selectable;
				_selected.Select();

				if (SelectedChangedEvent != null)
					SelectedChangedEvent(_selected);
			}

			_canMove = false;
			StartCoroutine(WaitToMove(MoveDelay));
		}

		/// <summary>
		/// Select objects based on input
		/// </summary>
		/// <param name="args"></param>
		public void ValueChanged(EventArgs<Vector2> args)
		{
			if (_canMove && Mathf.Abs(args.NewValue.x) > MoveThreshold)
			{
				if (_selected != null)
				{
					Selectable next = args.NewValue.x > 0 ? _selected.FindSelectableOnRight() : _selected.FindSelectableOnLeft();
					Select(args.Source, next);
				}
			}
		}

		/// <summary>
		/// Waits a given amount of time before we can move
		/// </summary>
		/// <param name="seconds">Number of seconds to wait</param>
		private IEnumerator WaitToMove(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			_canMove = true;
		}
	}
}
