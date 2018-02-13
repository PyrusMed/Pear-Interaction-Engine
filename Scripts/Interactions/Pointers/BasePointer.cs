using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pear.InteractionEngine.Interactions.Pointers
{
	public abstract class BasePointer : MonoBehaviour
	{
		/// <summary>
		/// Methods of when to consider a UI Click action
		/// </summary>
		/// <param name="ClickOnButtonUp">Consider a UI Click action has happened when the UI Click alias button is released.</param>
		/// <param name="ClickOnButtonDown">Consider a UI Click action has happened when the UI Click alias button is pressed.</param>
		public enum ClickMethods
		{
			ClickOnButtonUp,
			ClickOnButtonDown
		}

		[Tooltip("Determines when the UI Click event action should happen.")]
		public ClickMethods clickMethod = ClickMethods.ClickOnButtonUp;

		[Tooltip("Layer used to detect collisions")]
		public LayerMask CollisionLayers = 1 << 5; // UI by default

		// Fired when hover changes
		public event Action<GameObject, GameObject> HoverChangedEvent;

		// Fired when the pointer is activated or deactivated
		public event Action<bool> IsActiveChangedEvent;

		/// <summary>
		/// Tells whether this pointer is active or not
		/// </summary>
		bool _isActive = true;
		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				bool oldIsActive = _isActive;
				_isActive = value;
				if (IsActiveChangedEvent != null && oldIsActive != _isActive)
				{
					IsActiveChangedEvent(_isActive);
				}
			}
		}

		/// <summary>
		/// Pointer to the hovered element, if any
		/// </summary>
		private GameObject _hoveredElement = null;
		public virtual GameObject HoveredElement
		{
			get { return _hoveredElement; }
			set
			{
				GameObject oldHovered = _hoveredElement;
				_hoveredElement = value;
				if (HoverChangedEvent != null && oldHovered != _hoveredElement)
				{
					HoverChangedEvent(oldHovered, _hoveredElement);
				}
			}
		}

		/// <summary>
		/// The result of the raycast
		/// </summary>
		public abstract RaycastResult RaycastResult { get; }

		/// <summary>
		/// If true the pointer will click. Otherwise it will not.
		/// </summary>
		public bool Click { get; set; }

		protected virtual void OnEnable()
		{
			Click = false;
		}

		/// <summary>
		/// The GetOriginPosition method returns the relevant transform position for the pointer based on whether the pointerOriginTransform variable is valid.
		/// </summary>
		/// <returns>A Vector3 of the pointer transform position</returns>
		public virtual Vector3 GetOriginPosition()
		{
			return transform.position;
		}

		/// <summary>
		/// The GetOriginPosition method returns the relevant transform forward for the pointer based on whether the pointerOriginTransform variable is valid.
		/// </summary>
		/// <returns>A Vector3 of the pointer transform forward</returns>
		public virtual Vector3 GetOriginForward()
		{
			return transform.forward;
		}

		/// <summary>
		/// Tells whether the given game object is valid for this pointer
		/// </summary>
		/// <param name="obj">object to check</param>
		/// <returns></returns>
		public virtual bool IsValidElement(GameObject gameObject)
		{
			return CollisionLayers == (CollisionLayers | (1 << gameObject.layer));
		}
	}
}