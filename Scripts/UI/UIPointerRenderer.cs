using Pear.InteractionEngine.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// Renderer for the UI pointer
	/// </summary>
	[RequireComponent(typeof(UIPointer))]
	public class UIPointerRenderer : MonoBehaviour
	{
		[Tooltip("Width of the line")]
		public float LineWidth = 0.005f;

		[Tooltip("The colour to change the pointer materials when the pointer collides with a valid object. Set to `Color.clear` to bypass changing material colour on valid collision.")]
		public Color validCollisionColor = Color.green;

		[Tooltip("The colour to change the pointer materials when the pointer is not colliding with anything or with an invalid object. Set to `Color.clear` to bypass changing material colour on invalid collision.")]
		public Color invalidCollisionColor = Color.red;

		// Pointer
		private UIPointer _pointer;

		// Line renderer
		private LineRenderer _line;

		void Start() {
			_pointer = GetComponent<UIPointer>();
			_line = gameObject.AddComponent<LineRenderer>();
			_line.startWidth = _line.endWidth = LineWidth;

			_line.material.color = invalidCollisionColor;
			_pointer.HoverChangedEvent += (oldHovered, newHovered) =>
			{
				_line.material.color = newHovered != null ? validCollisionColor : invalidCollisionColor;
			};

			_pointer.IsActiveChangedEvent += isActive => _line.enabled = isActive;
		}

		/// <summary>
		/// Updates the pointer line
		/// </summary>
		void Update() {
			if (_pointer.IsActive)
			{
				Vector3 start = _pointer.GetOriginPosition();
				Vector3 end = _pointer.HoveredElement != null ?
					_pointer.pointerEventData.pointerCurrentRaycast.worldPosition :
					start + _pointer.GetOriginForward() * 10;

				_line.SetPositions(new Vector3[]
				{
					start,
					end,
				});
			}
		}
	}
}
