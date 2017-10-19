using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Converters;
using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// PIE's event system
	/// Almost a direction port from VTK
	/// https://github.com/thestonefox/VRTK/blob/25991cd6b9f935de07f2dcbd9ed1be82055e14f5/Assets/VRTK/Scripts/Internal/VRTK_EventSystem.cs
	/// </summary>
	public class Pear_EventSystem : EventSystem
	{
		// The previous event system used to keep track of state when enabling and disabling
		protected EventSystem _previousEventSystem;

		// The input module
		protected Pear_InputModule _inputModule;

		// Fields used to copy values
		private static readonly IEnumerable<FieldInfo> EVENT_SYSTEM_FIELD_INFOS = typeof(EventSystem).GetFields(BindingFlags.Public | BindingFlags.Instance);
		private static readonly PropertyInfo[] EVENT_SYSTEM_PROPERTY_INFOS = typeof(EventSystem).GetProperties(BindingFlags.Public | BindingFlags.Instance).Except(new[] { typeof(EventSystem).GetProperty("enabled") }).ToArray();
		private static readonly FieldInfo BASE_INPUT_MODULE_EVENT_SYSTEM_FIELD_INFO = typeof(BaseInputModule).GetField("m_EventSystem", BindingFlags.NonPublic | BindingFlags.Instance);

		protected override void Awake()
		{
			base.Awake();

			InitializeEvents();
		}

		/// <summary>
		/// Make sure each object that implements IEvent has the event object set
		/// </summary>
		public static void InitializeEvents()
		{
			// Loop over every object
			// If it's an event, set it's event property
			UnityEngine.Object[] objectsInScene = Resources.FindObjectsOfTypeAll(typeof(MonoBehaviour));
			Type iEventType = typeof(IEvent<>);
			Type propertyType = typeof(Property<>);
			int eventCounter = 0;
			foreach (MonoBehaviour mono in objectsInScene)
			{
				Type eventType = mono.GetType().GetInterfaces().FirstOrDefault(i =>
				{
#if WINDOWS_UWP
					bool isGenericType = i.GetTypeInfo().IsGenericType;
#else
					bool isGenericType = i.IsGenericType;
#endif
					return isGenericType && i.GetGenericTypeDefinition() == iEventType;
				});
				if (eventType != null)
				{
					// Create an instance of Property<T>
					Type eventValueType = eventType.GetGenericArguments()[0];
					Type eventPropertyType = propertyType.MakeGenericType(eventValueType);
					object eventPropertyValue = Activator.CreateInstance(eventPropertyType);

					// Set it on the script
					PropertyInfo eventProperty = eventType.GetProperty("Event");
					if (eventProperty.GetValue(mono, null) == null) {
						eventProperty.SetValue(mono, eventPropertyValue, null);
						eventCounter++;
					}
				}
			}

			Debug.Log(string.Format("[Pear Event System] Initialized {0} events", eventCounter));
		}

		/*
		public class NamedEventDispatcher<TEvent, TEventListener>
		{
			// Initial event value
			public TEvent DefaultEventValue = default(TEvent);

			private List<EventInfo> _events;

			private Dictionary<GameObject, List<EventListenerInfo>> _gameObjToListenersMap = new Dictionary<GameObject, List<EventListenerInfo>>();

			private List<EventListenerInfo> _alwaysReceiveEvents = new List<EventListenerInfo>();

			public void Add(IEvent<TEvent> namedEvent, Controller controller, IPropertyConverter<TEvent, TEventListener> converter)
			{
				Func<TEvent, TEventListener> convertFunc = eventVal => { return (TEventListener)(eventVal as object); };
				if (converter != null)
					convertFunc = converter.Convert;

				EventInfo info = new EventInfo
				{
					Event = namedEvent,
					Controller = controller,
					Converter = convertFunc,
				};

				_events.Add(info);

				namedEvent.Event.ValueChangeEvent += (TEvent oldValue, TEvent newValue) =>
				{
					EventValueChanged(oldValue, newValue, info);
				};
			}

			public void Add(IEventListener<TEventListener> listener, GameObject owner, ReceiveEventStates receiveEventState)
			{
				List<EventListenerInfo> listeners;
				if(!_gameObjToListenersMap.TryGetValue(owner, out listeners))
				{
					listeners = new List<EventListenerInfo>();
					_gameObjToListenersMap[owner] = listeners;
				}

				listeners.Add(new EventListenerInfo
				{
					Listener = listener,
					Owner = owner,
					ReceiveEventState = receiveEventState,
				}); 
			}

			private void EventValueChanged(TEvent oldValue, TEvent newValue, EventInfo eventInfo)
			{
				// Get a list of all listeners that need to hear about this event
				List<EventListenerInfo> listeners = new List<EventListenerInfo>();
				listeners.AddRange(_alwaysReceiveEvents);

				foreach (GameObject activeObject in eventInfo.Controller.ActiveObjects)
				{
					listeners.AddRange(_gameObjToListenersMap[activeObject]);
				}

				TEventListener oldValueForListener = eventInfo.Converter(oldValue);
				TEventListener newValueForListener = eventInfo.Converter(newValue);
				foreach (EventListenerInfo listenerInfo in listeners)
				{
					Dispatch(eventInfo.Controller, oldValueForListener, newValueForListener, listenerInfo);
				}
			}

			public void OnControllerActiveObjectsChanged(GameObject[] oldActiveObjects, GameObject[] newActiveObjects)
			{
				foreach(GameObject oldActiveObject in oldActiveObjects)
				{
					List<EventListenerInfo> listeners;
					if(_gameObjToListenersMap.TryGetValue(oldActiveObject, out listeners))
					{
						foreach(EventInfo eventInfo in _events)
						{
							TEventListener oldValueForListener = eventInfo.Converter(eventInfo.);
							TEventListener newValueForListener = convertFunc(newValue);
							foreach (EventListenerInfo listenerInfo in listeners)
							{
								Dispatch(controller, oldValueForListener, newValueForListener, listenerInfo);
							}
						}
					}
				}

				// If the listener is the old active object
				// make sure it receives the default, or inital, value as it's new value
				if (oldActiveObjects.Contains(_listenerGameObject))
				{
					Dispatch(
						oldValue: _event.Event.Value,
						newValue: DefaultEventValue);
				}
				// Otherwise, if it's the new active object,
				// make sure it's updated with the latest event value
				else if (newActiveObjects.Contains(_listenerGameObject))
				{
					Dispatch(
						oldValue: DefaultEventValue,
						newValue: _event.Event.Value);
				}
			})

			private void Dispatch(Controller controller, TEventListener oldValue, TEventListener newValue, EventListenerInfo listenerInfo)
			{
				if (listenerInfo.Owner == null)
					return;

				MonoBehaviour mono = listenerInfo.Listener as MonoBehaviour;
				if (mono && !mono.enabled)
					return;

				if (!Property<TEventListener>.AreEqual(oldValue, newValue))
				{
					EventArgs<TEventListener> eventArgs = new EventArgs<TEventListener>()
					{
						Source = controller,
						OldValue = oldValue,
						NewValue = newValue,
					};

					listenerInfo.Listener.ValueChanged(eventArgs);
				}
			}

			private class EventListenerInfo
			{
				public IEventListener<TEventListener> Listener;
				public GameObject Owner;
				public ReceiveEventStates ReceiveEventState;
			}

			private class EventInfo
			{
				public IEvent<TEvent> Event;
				public Controller Controller;
				public Func<TEvent, TEventListener> Converter;
			}
		}

		public static void LinkNamedEventsAndNamedEventHandlers()
		{
			// Get all named events
			NamedEvent[] namedEvents = Resources.FindObjectsOfTypeAll<NamedEvent>();
			if (!namedEvent.IsValid())
			{
				Debug.LogError("Invalid named event on object " + namedEvent.name);
				return;
			}

			NamedEventListener[] namedEventHandler = Resources.FindObjectsOfTypeAll<NamedEventListener>();
		}*/

		protected override void OnEnable()
		{
			_previousEventSystem = current;
			if (_previousEventSystem != null)
			{
				_previousEventSystem.enabled = false;
				CopyValuesFrom(_previousEventSystem, this);
			}

			_inputModule = transform.GetOrAddComponent<Pear_InputModule>();
			base.OnEnable();
			StartCoroutine(SetEventSystemOfBaseInputModulesAfterFrameDelay(this));
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			Destroy(_inputModule);

			if (_previousEventSystem != null)
			{
				_previousEventSystem.enabled = true;
				CopyValuesFrom(this, _previousEventSystem);
				SetEventSystemOfBaseInputModules(_previousEventSystem);
			}
		}

		protected override void Update()
		{
			base.Update();

			if (current == this)
			{
				_inputModule.Process();
			}
		}

		protected override void OnApplicationFocus(bool hasFocus)
		{
			//Don't call the base implementation because it will set a pause flag for this EventSystem
		}

		/// <summary>
		/// Copies values from one event system to another
		/// </summary>
		/// <param name="fromEventSystem">copy from</param>
		/// <param name="toEventSystem">copy to</param>
		private static void CopyValuesFrom(EventSystem fromEventSystem, EventSystem toEventSystem)
		{
			foreach (FieldInfo fieldInfo in EVENT_SYSTEM_FIELD_INFOS)
			{
				fieldInfo.SetValue(toEventSystem, fieldInfo.GetValue(fromEventSystem));
			}

			foreach (PropertyInfo propertyInfo in EVENT_SYSTEM_PROPERTY_INFOS)
			{
				if (propertyInfo.CanWrite)
				{
					propertyInfo.SetValue(toEventSystem, propertyInfo.GetValue(fromEventSystem, null), null);
				}
			}
		}

		/// <summary>
		/// Used to force update a private reference.
		/// </summary>
		/// <param name="eventSystem">event system to update</param>
		/// <returns></returns>
		private static IEnumerator SetEventSystemOfBaseInputModulesAfterFrameDelay(EventSystem eventSystem)
		{
			yield return null;
			SetEventSystemOfBaseInputModules(eventSystem);
		}

		/// <summary>
		/// BaseInputModule has a private field referencing the current EventSystem. That field is set in
		/// BaseInputModule.OnEnable only.It's used in BaseInputModule.OnEnable and BaseInputModule.OnDisable
		/// to call EventSystem.UpdateModules.
		/// This means we could just disable and enable every enabled BaseInputModule to fix that reference.
		/// But the StandaloneInputModule (which is added by default when adding an EventSystem in the Editor)
		/// requires EventSystem.Which means we can't correctly destroy the old EventSystem first and then add
		/// our own one.
		/// We therefore update that private reference directly here.
		/// </summary>
		/// <param name="eventSystem"></param>
		private static void SetEventSystemOfBaseInputModules(EventSystem eventSystem)
		{
			foreach (BaseInputModule module in FindObjectsOfType<BaseInputModule>())
			{
				BASE_INPUT_MODULE_EVENT_SYSTEM_FIELD_INFO.SetValue(module, eventSystem);
			}

			eventSystem.UpdateModules();
		}
	}
}