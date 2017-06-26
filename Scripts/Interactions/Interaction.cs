using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Properties.Converters;
using Pear.InteractionEngine.Utils;
using System;
using System.Reflection;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// The glue that stitches Events and EventHandlers. This class allows us to mix and match events that modify a certain
	/// type of property with event handlers that react to that same property type.
	/// 
	/// Unfortunately, the code is a little hairy because Unity doesn't play nice with generic classes. Nonetheless, we made it happen.
	/// I'll try and comment verbosely.
	/// </summary>
	public class Interaction : MonoBehaviour
	{

		// Script that modifies a property
		[SerializeField]
		private MonoBehaviour Event;

		// Script that converts the property from the event's
		// type into the EventHandler's type
		[SerializeField]
		private MonoBehaviour ValueConverter;

		// Script that modifies a property
		[SerializeField]
		private Controller EventController;

		// Reacts to the event's modifications
		[SerializeField]
		private MonoBehaviour EventHandler;

		[SerializeField]
		private string EventPropertyType;

		[SerializeField]
		private string EventHandlerPropertyType;

		[SerializeField]
		private ReceiveEventStates ReceiveEventState = ReceiveEventStates.WhenObjectActive;

		// Typed helper that makes the logic in this class easier to understand
		private IInteractionHelper _interactionHelper;

		void Awake()
		{
			if (!IsValid())
			{
				Debug.LogError("Could not create interaction for GameObject " + name);
				return;
			}

			// Instantiate the property typed instance of our helper class so we don't have to use reflection to do everything
			Type eventPropertyType = Type.GetType(EventPropertyType);
			Type eventHandlerPropertyType = Type.GetType(EventHandlerPropertyType);
			Type interactionHelperType = typeof(InteractionHelper<,>);
			Type instantiableInteractionHelperType = interactionHelperType.MakeGenericType(eventPropertyType, eventHandlerPropertyType);
			_interactionHelper = (IInteractionHelper)Activator.CreateInstance(instantiableInteractionHelperType, Event, EventHandler, ValueConverter, gameObject, EventController);

			// Create a new property and register it with Event and EventHandler
			_interactionHelper.EventingEnabled = true;

			// Define when this interaction should receieve events
			_interactionHelper.ReceiveEventState = ReceiveEventState;
		}

		/// <summary>
		/// Copies all game objects from one object to another
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
				Debug.LogError(String.Format("This interaction needs a value converter. '{0}' -> '{1}'", EventPropertyType, EventHandlerPropertyType));
				return false;
			}

			if (EventPropertyType == EventHandlerPropertyType && ValueConverter != null)
			{
				Debug.LogError(String.Format("This interaction has a converter when it doesn't need one. '{0}' -> '{1}'", EventPropertyType, EventHandlerPropertyType));
				return false;
			}

			return true;
		}

		/// <summary>
		/// When the script is enabled make sure the properties are registered
		/// </summary>
		private void OnEnable()
		{
			if (_interactionHelper != null)
				_interactionHelper.EventingEnabled = true;
		}

		/// <summary>
		/// When the script is diosabled make sure the properties are unregistered
		/// </summary>
		private void OnDisable()
		{
			if (_interactionHelper != null)
				_interactionHelper.EventingEnabled = false;
		}

		/// <summary>
		/// When this script is destroyed unregister the property with the Event and EventHandler
		/// </summary>
		void OnDestroy()
		{
			if (_interactionHelper != null)
				_interactionHelper.EventingEnabled = false;
		}
	}

	/// <summary>
	/// Helper interface that makes modifying the Event and EventHandler easier
	/// </summary>
	internal interface IInteractionHelper
	{
		bool EventingEnabled { get; set; }
		ReceiveEventStates ReceiveEventState { get; set; }
	}

	/// <summary>
	/// With this class we can reference the Event and EventHandler using strongly typed objects.
	/// This makes working with variables much easier. If not for this class we would have to use
	/// reflection to get things done :/ :).
	/// </summary>
	/// <typeparam name="TEvent">Type of value the event emits</typeparam>
	/// <typeparam name="TEventListener">Type of value the listener receives</typeparam>
	internal class InteractionHelper<TEvent, TEventListener> : IInteractionHelper
	{
		private EventDispatcher<TEvent, TEventListener> _dispatcher;

		public bool EventingEnabled
		{
			get { return _dispatcher.Enabled; }
			set { _dispatcher.Enabled = value; }
		}

		public ReceiveEventStates ReceiveEventState
		{
			get { return _dispatcher.ReceiveEventState; }
			set { _dispatcher.ReceiveEventState = value; }
		}

		public InteractionHelper(InteractionEngine.Events.IEvent<TEvent> ev,
			IEventListener<TEventListener> listener,
			IPropertyConverter<TEvent, TEventListener> converter,
			GameObject go,
			Controller eventController)
		{

			ev.Event = ev.Event ?? new Property<TEvent>();
			_dispatcher = new EventDispatcher<TEvent, TEventListener>(eventController, go, ev, converter, listener);
		}
	}

	public enum ReceiveEventStates
	{
		WhenObjectActive,
		Always,
	}

	public static class InteractionExtensions
	{
		public static T GetCopyOf<T>(this Component comp, T other) where T : Component
		{
			Type type = comp.GetType();
			if (type != other.GetType()) return null; // type mis-match
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
			PropertyInfo[] pinfos = type.GetProperties(flags);
			foreach (var pinfo in pinfos)
			{
				if (pinfo.CanWrite)
				{
					try
					{
						pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
					}
					catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
				}
			}
			FieldInfo[] finfos = type.GetFields(flags);
			foreach (var finfo in finfos)
			{
				finfo.SetValue(comp, finfo.GetValue(other));
			}
			return comp as T;
		}

		public static T AddComponentFrom<T>(this GameObject go, T from) where T : Component
		{
			if (from == null)
				return null;

			return go.AddComponent(from.GetType()).GetCopyOf(from) as T;
		}
	}
}
