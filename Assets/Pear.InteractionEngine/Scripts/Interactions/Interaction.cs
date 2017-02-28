using Pear.InteractionEngine.Properties;
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
	public class Interaction : MonoBehaviour {

		// Script that modifies a property
		[SerializeField]
		private MonoBehaviour Event;

		// Reacts to the event's modifications
		[SerializeField]
		private MonoBehaviour EventHandler;

		// Typed helper that makes the logic in this class easier to understand
		private IInteractionHelper _interactionHelper;

		void Start() {
			if (Event == null || EventHandler == null)
			{
				Debug.LogError("Both the Event and EventHandler need to be set.");
				return;
			}

			// Make sure the Event and EventHandler reference the same property type
			// If one works with, say, a boolean property and one with, say, a string property, we'll have serious issues
			Type eventPropertyType = ReflectionHelpers.GetGenericArgumentTypes(Event.GetType(), typeof(IGameObjectPropertyEvent<>))[0];
			Type eventHandlerPropertyType = ReflectionHelpers.GetGenericArgumentTypes(EventHandler.GetType(), typeof(IGameObjectPropertyEventHandler<>))[0];
			if(eventPropertyType != eventHandlerPropertyType)
			{
				Debug.LogError("Interaction Event and EventHandler types do not match up");
				return;
			}

			// Instantiate the properly typed instance of our helper class so we don't have to use reflection to do everything
			Type interactionHelperType = typeof(InteractionHelper<>);
			Type instantiableInteractionHelperType = interactionHelperType.MakeGenericType(eventPropertyType);
			_interactionHelper = (IInteractionHelper)Activator.CreateInstance(instantiableInteractionHelperType, Event, EventHandler, gameObject);

			// Create a new property and register it with Event and EventHandler
			_interactionHelper.RegisterProperty();
		}

		/// <summary>
		/// When this script is destroyed unregister the property with the Event and EventHandler
		/// </summary>
		void OnDestroy()
		{
			if(_interactionHelper != null)
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
	internal class InteractionHelper<T> : IInteractionHelper
	{
		// The event
		private IGameObjectPropertyEvent<T> _event;

		// The event handler
		private IGameObjectPropertyEventHandler<T> _eventHandler;

		// The property 
		private GameObjectProperty<T> _property;

		public InteractionHelper(IGameObjectPropertyEvent<T> ev, IGameObjectPropertyEventHandler<T> evHandler, GameObject go)
		{
			_event = ev;
			_eventHandler = evHandler;
			_property = new GameObjectProperty<T>(go);
		}

		/// <summary>
		/// Register the property with the Event and EventHandler
		/// </summary>
		public void RegisterProperty()
		{
			_eventHandler.RegisterProperty(_property);
			_event.RegisterProperty(_property);
		}

		/// <summary>
		/// Unregister the property with the Event and EventHandler
		/// </summary>
		public void UnregisterProperty()
		{
			_event.UnregisterProperty(_property);
			_eventHandler.UnregisterProperty(_property);
		}
	}
}
