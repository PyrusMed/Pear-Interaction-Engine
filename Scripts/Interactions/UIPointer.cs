using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Points at UI elements
	/// </summary>
	public class UIPointer : MonoBehaviour
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

		[Tooltip("A custom transform to use as the origin of the pointer. If no pointer origin transform is provided then the transform the script is attached to is used.")]
		public Transform pointerOriginTransform = null;

		// Data about where and what the pointer is pointing at
		public PointerEventData pointerEventData;

		// Fired when hover changes
		public event Action<GameObject, GameObject> HoverChangedEvent;

		// Fired when the pointer is activated or deactivated
		public event Action<bool> IsActiveChangedEvent;

		// Saves a pointer the the event system
		protected EventSystem cachedEventSystem;

		// Saves a pointer to the input module
		protected Pear_InputModule cachedVRInputModule;

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
				if(IsActiveChangedEvent != null && oldIsActive != _isActive)
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
				if(HoverChangedEvent != null && oldHovered != _hoveredElement)
				{
					HoverChangedEvent(oldHovered, _hoveredElement);
				}
			}
		}

		/// <summary>
		/// If true the pointer will click. Otherwise it will not.
		/// </summary>
		public bool Click {	get; set; }

		protected virtual void OnEnable()
		{
			ConfigureEventSystem();
			Click = false;
		}

		protected virtual void OnDisable()
		{
			if (cachedVRInputModule && cachedVRInputModule.pointers.Contains(this))
				cachedVRInputModule.pointers.Remove(this);
		}

		/// <summary>
		/// The GetOriginPosition method returns the relevant transform position for the pointer based on whether the pointerOriginTransform variable is valid.
		/// </summary>
		/// <returns>A Vector3 of the pointer transform position</returns>
		public virtual Vector3 GetOriginPosition()
		{
			return (pointerOriginTransform ? pointerOriginTransform.position : transform.position);
		}

		/// <summary>
		/// The GetOriginPosition method returns the relevant transform forward for the pointer based on whether the pointerOriginTransform variable is valid.
		/// </summary>
		/// <returns>A Vector3 of the pointer transform forward</returns>
		public virtual Vector3 GetOriginForward()
		{
			return (pointerOriginTransform ? pointerOriginTransform.forward : transform.forward);
		}

		/// <summary>
		/// The SetEventSystem method is used to set up the global Unity event system for the UI pointer. It also handles disabling the existing Standalone Input Module that exists on the EventSystem and adds a custom VRTK Event System VR Input component that is required for interacting with the UI with VR inputs.
		/// </summary>
		/// <param name="eventSystem">The global Unity event system to be used by the UI pointers.</param>
		/// <returns>A custom input module that is used to detect input from VR pointers.</returns>
		public virtual Pear_InputModule SetEventSystem(EventSystem eventSystem)
		{
			if (!eventSystem)
			{
				Debug.LogError("EventSystem missing from scene");
				return null;
			}

			if (!(eventSystem is Pear_EventSystem))
			{
				eventSystem = eventSystem.gameObject.AddComponent<Pear_EventSystem>();
			}

			return eventSystem.transform.GetOrAddComponent<Pear_InputModule>();
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

		/// <summary>
		/// Makes sure all the required components are present
		/// and caches important data
		/// </summary>
		protected virtual void ConfigureEventSystem()
		{
			if (cachedEventSystem == null)
				cachedEventSystem = FindObjectOfType<EventSystem>();

			if (cachedVRInputModule == null)
				cachedVRInputModule = SetEventSystem(cachedEventSystem);

			if (cachedEventSystem != null && cachedVRInputModule != null)
			{
				if (pointerEventData == null)
					pointerEventData = new PointerEventData(cachedEventSystem);

				if (!cachedVRInputModule.pointers.Contains(this))
					cachedVRInputModule.pointers.Add(this);
			}
		}
	}
}