using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Interactions;
using Pear.InteractionEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class NamedEventListener : MonoBehaviour
	{
		// Name of the event
		public string EventName;

		// Reacts to the event
		public MonoBehaviour EventListener;

		// Used for serialization, this variable stores
		// the type of property (e.g. float, bool, ect)
		// that the event listener fires
		public string EventListenerPropertyType;

		// Determines when the listener should recieve events
		public ReceiveEventStates ReceiveEventState = ReceiveEventStates.WhenObjectActive;

		private IEventListenerDispatcher _eventListenerDispatcher;

		void Awake()
		{
			if (!IsValid())
			{
				Debug.LogError("Named event is invalid for object " + name);
				return;
			}

			if (EventListener.gameObject != gameObject)
				CopyEventListener(EventListener);

			Type eventListnerPropertyType = Type.GetType(EventListenerPropertyType);
			Type eventListenerDispatcherType = typeof(EventListenerDispatcher<>);
			Type instantiableEventListenerDispatcherType = eventListenerDispatcherType.MakeGenericType(eventListnerPropertyType);
			_eventListenerDispatcher = (IEventListenerDispatcher)Activator.CreateInstance(instantiableEventListenerDispatcherType, EventName, EventListener, gameObject, ReceiveEventState);
		}

		/// <summary>
		/// Copies all named event listeners from one object to another
		/// </summary>
		/// <param name="copyFrom">Obj to copy from</param>
		/// <param name="copyTo">Obj to copy to</param>
		public static void CopyAll(GameObject copyFrom, GameObject copyTo)
		{
			// Deactivate this object so we can copy component values without
			// the component's Awake function being called
			bool originalActiveState = copyTo.activeSelf;
			copyTo.SetActive(false);

			// Copy listeners from our placeholder
			foreach (NamedEventListener interaction in copyFrom.GetComponents<NamedEventListener>())
			{
				NamedEventListener newInteraction = copyTo.AddComponent<NamedEventListener>();
				newInteraction.CopyFrom(interaction);
			}

			// Reset the original state
			copyTo.SetActive(originalActiveState);
		}

		/// <summary>
		/// Copy listener properties from the given listener
		/// </summary>
		/// <param name="copyFrom">listener to copy properties from</param>
		public void CopyFrom(NamedEventListener copyFrom)
		{
			if (copyFrom == null || !copyFrom.IsValid())
			{
				UnityEngine.Debug.LogError("Invalid interaction to copy from.");
				return;
			}

			EventName = copyFrom.EventName;
			CopyEventListener(copyFrom.EventListener);
			EventListenerPropertyType = copyFrom.EventListenerPropertyType;
			ReceiveEventState = copyFrom.ReceiveEventState;
		}

		private void OnEnable()
		{
			_eventListenerDispatcher.Enabled = true;
		}

		private void OnDisable()
		{
			_eventListenerDispatcher.Enabled = false;
		}

		private void CopyEventListener(MonoBehaviour eventListener)
		{
			// If the event listener is a singleton we just want to point to it
			// if it is NOT a singleton duplicate the script
			bool isEventListenerSingleton = eventListener.DerivesFromGeneric(typeof(Singleton<>));
			EventListener = isEventListenerSingleton ? eventListener : gameObject.AddComponentFrom(eventListener);
		}

		/// <summary>
		/// Tells whether this event listener is valid
		/// </summary>
		/// <returns>True if event listener is valid. False otherwise.</returns>
		private bool IsValid()
		{
			if (EventListener == null)
			{
				Debug.LogError("The EventHandler needs to be set.");
				return false;
			}

			if (EventListenerPropertyType == null)
			{
				Debug.LogError("EventListener property type not set.");
				return false;
			}

			Type eventListenerPropertyType = Type.GetType(EventListenerPropertyType);
			if (eventListenerPropertyType == null)
			{
				Debug.LogError("EventListener property type is invalid or null.");
				return false;
			}

			return true;
		}

		public interface IEventListenerDispatcher
		{
			bool Enabled { get; set; }
		}

		public class EventListenerDispatcher<T> : IEventListenerDispatcher
		{
			private string EventName;
			private IEventListener<T> EventListener;
			private GameObject GameObject;
			public ReceiveEventStates ReceiveEventState;

			private bool _enabled = false;
			public bool Enabled
			{
				get { return _enabled; }

				set
				{
					if(value != _enabled)
					{
						if(value)
						{
							EventToEventListenerDispatcher<T>.AddListener(EventName, EventListener, GameObject, ReceiveEventState);
						}
						else
						{
							EventToEventListenerDispatcher<T>.RemoveListener(EventName, EventListener, GameObject);
						}
					}

					_enabled = value;
				}
			}

			public EventListenerDispatcher(string eventName, IEventListener<T> eventListener, GameObject gameObject, ReceiveEventStates receiveEventState)
			{
				EventName = eventName;
				EventListener = eventListener;
				GameObject = gameObject;
				ReceiveEventState = receiveEventState;
			}			
		}
	}
}
