using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Utils;
using System;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// The glue that stitches Events and EventListeners. This class allows us to attach any event listener to any event
	/// as long as there is a way to convert from the event's type to the event listener's type.
	/// 
	/// Unfortunately, the code is a little hairy because Unity doesn't play nice with generic classes. Nonetheless, we made it happen!
	/// I'll try and comment verbosely.
	/// </summary>
	public class Interaction : MonoBehaviour
	{
		/// <summary>
		/// Defines when an object receives events
		/// </summary>
		public enum ReceiveEventStates
		{
			WhenObjectActive,
			Always,
		}

		// Fires the event
		[SerializeField]
		private MonoBehaviour Event;

		// Converts the event's value into a value
		// the event lister understands
		[SerializeField]
		private MonoBehaviour ValueConverter;

		// The controller that emits the event
		[SerializeField]
		private Controller EventController;

		// Reacts to the event
		[SerializeField]
		private MonoBehaviour EventHandler;

		// Used for serialization, this variable stores
		// the type of property (e.g. float, bool, ect)
		// that the event fires
		[SerializeField]
		private string EventPropertyType;

		// Used for serialization, this variable stores
		// the type of property (e.g. float, bool, ect)
		// that the event listener fires
		[SerializeField]
		private string EventHandlerPropertyType;

		// Determines when the listener should recieve events
		[SerializeField]
		private ReceiveEventStates ReceiveEventState = ReceiveEventStates.WhenObjectActive;

		// Helps dispatch events
		private IEventDispatcher _eventDispatcher;

		void Start()
		{
			// If this interaction is not valid show an error
			if (!IsValid())
			{
				Debug.LogError("Could not create interaction for GameObject " + name);
				return;
			}

			// Instantiate the property typed instance of our helper class so we don't have to use reflection to do everything
			Type eventPropertyType = Type.GetType(EventPropertyType);
			Type eventHandlerPropertyType = Type.GetType(EventHandlerPropertyType);
			Type eventDispatcherType = typeof(EventDispatcher<,>);
			Type instantiableEventDispatcherType = eventDispatcherType.MakeGenericType(eventPropertyType, eventHandlerPropertyType);
			_eventDispatcher = (IEventDispatcher)Activator.CreateInstance(instantiableEventDispatcherType, EventController, gameObject, Event, ValueConverter, EventHandler);

			// Make sure eventing is enabled
			_eventDispatcher.EventingEnabled = true;

			// Define when the listener should receieve events
			_eventDispatcher.ReceiveEventState = ReceiveEventState;
		}

		/// <summary>
		/// Copies all interactions from one object to another
		/// </summary>
		/// <param name="copyFrom">Obj to copy from</param>
		/// <param name="copyTo">Obj to copy to</param>
		public static void CopyAll(GameObject copyFrom, GameObject copyTo)
		{
			// Deactivate this object so we can copy component values without
			// the component's Awake function being called
			bool originalActiveState = copyTo.activeSelf;
			copyTo.SetActive(false);

			// Copy interactions from our placeholder
			foreach (Interaction interaction in copyFrom.GetComponents<Interaction>())
			{
				Interaction newInteraction = copyTo.AddComponent<Interaction>();
				newInteraction.CopyFrom(interaction);
			}

			// Reset the original state
			copyTo.SetActive(originalActiveState);
		}

		/// <summary>
		/// Copy interaction properties from the given Interaction
		/// </summary>
		/// <param name="interactionToUpdate">Interaction to copy properties from</param>
		private void CopyFrom(Interaction copyFrom)
		{
			if (copyFrom == null || !copyFrom.IsValid())
			{
				Debug.LogError("Invalid interaction to copy from.");
				return;
			}

			Event = copyFrom.Event;
			ValueConverter = gameObject.AddComponentFrom(copyFrom.ValueConverter);
			EventHandler = gameObject.AddComponentFrom(copyFrom.EventHandler);
			EventController = copyFrom.EventController;
			EventPropertyType = copyFrom.EventPropertyType;
			EventHandlerPropertyType = copyFrom.EventHandlerPropertyType;
			ReceiveEventState = copyFrom.ReceiveEventState;
		}

		/// <summary>
		/// Tells whether this interaction is valid
		/// </summary>
		/// <returns>True if interaction is valid. False otherwise.</returns>
		private bool IsValid()
		{
			if (Event == null || EventHandler == null)
			{
				Debug.LogError("Both the Event and EventHandler need to be set.");
				return false;
			}

			if (EventPropertyType == null || EventHandlerPropertyType == null)
			{
				Debug.LogError("Interaction property type not set.");
				return false;
			}

			Type eventPropertyType = Type.GetType(EventPropertyType);
			Type eventHandlerPropertyType = Type.GetType(EventHandlerPropertyType);
			if(eventPropertyType == null || eventHandlerPropertyType == null)
			{
				Debug.LogError(string.Format("Event or handler property type is null. Event '{0}'. EventHandler '{1}'",
					eventPropertyType == null ? null : eventPropertyType.Name,
					eventHandlerPropertyType == null ? null : eventHandlerPropertyType.Name));
				return false;
			}

			if (EventPropertyType != EventHandlerPropertyType && ValueConverter == null)
			{
				Debug.LogError(String.Format("[{0}] This interaction needs a value converter. '{1}' -> '{2}'", name, EventPropertyType, EventHandlerPropertyType));
				return false;
			}

			return true;
		}

		/// <summary>
		/// When the script is enabled make sure the properties are registered
		/// </summary>
		private void OnEnable()
		{
			if (_eventDispatcher != null)
				_eventDispatcher.EventingEnabled = true;
		}

		/// <summary>
		/// When the script is diosabled make sure the properties are unregistered
		/// </summary>
		private void OnDisable()
		{
			if (_eventDispatcher != null)
				_eventDispatcher.EventingEnabled = false;
		}

		/// <summary>
		/// When this script is destroyed unregister the property with the Event and EventHandler
		/// </summary>
		void OnDestroy()
		{
			if (_eventDispatcher != null)
				_eventDispatcher.EventingEnabled = false;
		}
	}
}
