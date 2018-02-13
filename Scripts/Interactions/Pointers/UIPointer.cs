using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions.Pointers
{
	/// <summary>
	/// Points at UI elements
	/// </summary>
	public class UIPointer : BasePointer
	{
		// Data about where and what the pointer is pointing at
		public PointerEventData pointerEventData;

		// Saves a pointer the the event system
		protected EventSystem cachedEventSystem;

		// Saves a pointer to the input module
		protected Pear_InputModule cachedVRInputModule;

		/// <summary>
		/// The result of the raycast
		/// </summary>
		public override RaycastResult RaycastResult { get { return pointerEventData.pointerCurrentRaycast; } }

		protected override void OnEnable()
		{
			ConfigureEventSystem();
			base.OnEnable();
		}

		protected virtual void OnDisable()
		{
			if (cachedVRInputModule && cachedVRInputModule.pointers.Contains(this))
				cachedVRInputModule.pointers.Remove(this);
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