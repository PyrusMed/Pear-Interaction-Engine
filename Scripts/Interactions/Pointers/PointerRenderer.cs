using Pear.InteractionEngine.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.Pointers
{
	/// <summary>
	/// Renderer for the UI pointer
	/// </summary>
	public class PointerRenderer : MonoBehaviour
	{
		[Tooltip("The BasePointer that this renderer is based on")]
		public BasePointer Pointer;

		[Tooltip("Width of the line")]
		public float LineWidth = 0.005f;

		[Tooltip("The colour to change the pointer materials when the pointer collides with a valid object. Set to `Color.clear` to bypass changing material colour on valid collision.")]
		public Color validCollisionColor = Color.green;

		[Tooltip("The colour to change the pointer materials when the pointer is not colliding with anything or with an invalid object. Set to `Color.clear` to bypass changing material colour on invalid collision.")]
		public Color invalidCollisionColor = Color.red;

		// Line renderer
		private LineRenderer _line;

		void Start() {
			// Setup the line
			_line = gameObject.AddComponent<LineRenderer>();
			_line.startWidth = _line.endWidth = LineWidth;
			_line.material.color = invalidCollisionColor;

			// Listen for pointer events
			Pointer.HoverChangedEvent += (oldHovered, newHovered) =>
			{
				_line.material.color = newHovered != null ? validCollisionColor : invalidCollisionColor;
			};

			Pointer.IsActiveChangedEvent += isActive => _line.enabled = isActive;
		}

		/// <summary>
		/// Updates the pointer line
		/// </summary>
		void Update() {
			if (Pointer.IsActive)
			{
				Vector3 start = Pointer.GetOriginPosition();
				Vector3 end = Pointer.HoveredElement != null ?
					Pointer.RaycastResult.worldPosition :
					start + Pointer.GetOriginForward() * 10;

				_line.SetPositions(new Vector3[]
				{
					start,
					end,
				});
			}
		}

		private void OnEnable()
		{
			if(_line != null)
				_line.enabled = true;
		}

		private void OnDisable()
		{
			_line.enabled = false;
		}
	}
}
