using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Properties.Converters;
using Pear.InteractionEngine.Utils;
using System;
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

		// Typed helper that makes the logic in this class easier to understand
		private IInteractionHelper _interactionHelper;

		void Start()
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
			_interactionHelper.RegisterProperty();
		}

		/// <summary>
		/// Copies all game objects from one object to another
		/// </summary>
		/// <param name="copyFrom">Obj to copy from</param>
		/// <param name="copyTo">Obj to copy to</param>
		public static void CopyAll(GameObject copyFrom, GameObject copyTo)
		{
			// Copy interactions from our placeholder
			foreach (Interaction interaction in copyFrom.GetComponents<Interaction>())
			{
				Interaction newInteraction = copyTo.AddComponent<Interaction>();
				newInteraction.CopyFrom(interaction);
			}
		}

		/// <summary>
		/// Copy interaction properties from the given Interaction
		/// </summary>
		/// <param name="interactionToUpdate">Interaction to copy properties from</param>
		public void CopyFrom(Interaction copyFrom)
		{
			if (copyFrom == null || !copyFrom.IsValid())
			{
				Debug.LogError("Invalid interaction to copy from.");
				return;
			}

			Event = copyFrom.Event;
			ValueConverter = copyFrom.ValueConverter;
			EventHandler = copyFrom.EventHandler;
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
				_interactionHelper.RegisterProperty();
		}

		/// <summary>
		/// When the script is diosabled make sure the properties are unregistered
		/// </summary>
		private void OnDisable()
		{
			if (_interactionHelper != null)
				_interactionHelper.UnregisterProperty();
		}

		/// <summary>
		/// When this script is destroyed unregister the property with the Event and EventHandler
		/// </summary>
		void OnDestroy()
		{
			if (_interactionHelper != null)
				_interactionHelper.UnregisterProperty();
		}
	}

	/// <summary>
	/// Helper interface that makes modifying the Event and EventHandler easier
	/// </summary>
	internal interface IInteractionHelper
	{
		void RegisterProperty();
		void UnregisterProperty();
	}

	/// <summary>
	/// With this class we can reference the Event and EventHandler using strongly typed objects.
	/// This makes working with variables much easier. If not for this class we would have to use
	/// reflection to get things done :/ :).
	/// </summary>
	/// <typeparam name="T">Type of property</typeparam>
	internal class InteractionHelper<TEvent, TEventHandler> : IInteractionHelper
	{
		// The event
		private IGameObjectPropertyEvent<TEvent> _event;

		// The event handler
		private IGameObjectPropertyEventHandler<TEventHandler> _eventHandler;

		// The event's property 
		private GameObjectProperty<TEvent> _eventProperty;

		// The event handler's property 
		private GameObjectProperty<TEventHandler> _eventHandlerProperty;

		// Tracks whether the property is registered
		private bool _registered = false;

		public InteractionHelper(IGameObjectPropertyEvent<TEvent> ev,
			IGameObjectPropertyEventHandler<TEventHandler> evHandler,
			IPropertyConverter<TEvent, TEventHandler> converter,
			GameObject go,
			Controller eventController)
		{
			_event = ev;
			_eventHandler = evHandler;

			// If there's a converter, convert from the event type
			// to the event handler's type
			if(converter != null)
			{
				_eventProperty = new GameObjectProperty<TEvent>(go, eventController);
				_eventHandlerProperty = new GameObjectProperty<TEventHandler>(go, eventController);

				// Convert from the event type to the handler type when the event's value changes
				_eventProperty.ChangeEvent += (oldValue, newValue) =>
				{
					_eventHandlerProperty.Value = converter.Convert(newValue);
				};
			}
			else
			{
				_eventProperty = new GameObjectProperty<TEvent>(go, eventController);
				_eventHandlerProperty = _eventProperty as GameObjectProperty<TEventHandler>;
			}
			
		}

		/// <summary>
		/// Register properties with the Event and EventHandler
		/// </summary>
		public void RegisterProperty()
		{
			if (!_registered)
			{
				_eventHandler.RegisterProperty(_eventHandlerProperty);
				_event.RegisterProperty(_eventProperty);
				_registered = true;
			}
		}

		/// <summary>
		/// Unregister properties with the Event and EventHandler
		/// </summary>
		public void UnregisterProperty()
		{
			if (_registered)
			{
				_event.UnregisterProperty(_eventProperty);
				_eventHandler.UnregisterProperty(_eventHandlerProperty);
				_registered = false;
			}
		}
	}
}
