using Pear.InteractionEngine.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactables {
	public class Interaction : MonoBehaviour {

		[SerializeField]
		private MonoBehaviour Event;

		[SerializeField]
		private MonoBehaviour EventHandler;

		private IInteractionHelper _interactionHelper;

		void Start() {
			if (Event == null || EventHandler == null)
				return;

			Type eventPropertyType = ReflectionHelpers.GetGenericArgumentType(Event.GetType(), typeof(IGameObjectPropertyEvent<>))[0];
			Type eventHandlerPropertyType = ReflectionHelpers.GetGenericArgumentType(EventHandler.GetType(), typeof(IGameObjectPropertyEventHandler<>))[0];
			if(eventPropertyType != eventHandlerPropertyType)
			{
				Debug.LogError("Interaction event and event handler types do not match up");
				return;
			}

			Type interactionHelperType = typeof(InteractionHelper<>);
			Type instantiableInteractionHelperType = interactionHelperType.MakeGenericType(eventPropertyType);
			_interactionHelper = (IInteractionHelper)Activator.CreateInstance(instantiableInteractionHelperType, Event, EventHandler, gameObject);

			_interactionHelper.RegisterProperty();
		}

		void OnDestroy()
		{
			_interactionHelper.UnregisterProperty();
		}

		
	}

	public interface IInteractionHelper
	{
		void RegisterProperty();
		void UnregisterProperty();
	}

	public class InteractionHelper<T> : IInteractionHelper
	{
		private IGameObjectPropertyEvent<T> _event;
		private IGameObjectPropertyEventHandler<T> _eventHandler;
		private GameObjectProperty<T> _property;

		public InteractionHelper(IGameObjectPropertyEvent<T> ev, IGameObjectPropertyEventHandler<T> evHandler, GameObject go)
		{
			_event = ev;
			_eventHandler = evHandler;
			_property = new GameObjectProperty<T>(go);
		}

		public void RegisterProperty()
		{
			_eventHandler.RegisterProperty(_property);
			_event.RegisterProperty(_property);
		}

		public void UnregisterProperty()
		{
			_event.UnregisterProperty(_property);
			_eventHandler.UnregisterProperty(_property);
		}
	}
}
