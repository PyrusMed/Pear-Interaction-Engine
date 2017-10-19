using Pear.InteractionEngine.EventListeners;
using Pear.InteractionEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public static class EventToEventListenerDispatcher<TEventListener>
	{
		private static Dictionary<string, EventListenerCollection> _eventNameToListeners = new Dictionary<string, EventListenerCollection>();

		public static void AddListener(string eventName, IEventListener<TEventListener> listener, GameObject owner, ReceiveEventStates receiveEventState)
		{
			EventListenerCollection listeners;
			if (!_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
				listeners = new EventListenerCollection();
				_eventNameToListeners[eventName] = listeners;
			}

			listeners.Add(listener, owner, receiveEventState);
		}

		public static void RemoveListener(string eventName, IEventListener<TEventListener> listener, GameObject owner)
		{
			EventListenerCollection listeners;
			if (!_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
				listeners.Remove(listener, owner);
			}
		}

		public static void DispatchToListeners(string eventName, EventArgs<TEventListener> args)
		{
			EventListenerCollection listeners;
			if (_eventNameToListeners.TryGetValue(eventName, out listeners))
			{
				foreach (IEventListener<TEventListener> listener in listeners.GetListeners(args.Source.ActiveObjects))
				{
					listener.ValueChanged(args);
				}
			}
			else
			{

			}
		}

		private class EventListenerCollection
		{
			private Dictionary<GameObject, List<EventListenerInfo>> _gameObjToListenersMap = new Dictionary<GameObject, List<EventListenerInfo>>();

			private List<EventListenerInfo> _alwaysReceiveEvents = new List<EventListenerInfo>();

			public void Add(IEventListener<TEventListener> listener, GameObject owner, ReceiveEventStates receiveEventState)
			{
				EventListenerInfo info = new EventListenerInfo
				{
					Listener = listener,
					Owner = owner,
					ReceiveEventState = receiveEventState,
				};

				if (receiveEventState == ReceiveEventStates.Always)
				{
					_alwaysReceiveEvents.Add(info);
				}
				else
				{
					List<EventListenerInfo> listeners;
					if (!_gameObjToListenersMap.TryGetValue(owner, out listeners))
					{
						listeners = new List<EventListenerInfo>();
						_gameObjToListenersMap[owner] = listeners;
					}

					listeners.Add(info);
				}				
			}

			public void Remove(IEventListener<TEventListener> listener, GameObject owner)
			{
				List<EventListenerInfo> listeners;
				if (_gameObjToListenersMap.TryGetValue(owner, out listeners))
				{
					for(int infoIndex = 0; infoIndex < listeners.Count; infoIndex++)
					{
						EventListenerInfo info = listeners[infoIndex];
						if(info.Listener == listener)
						{
							listeners.Remove(info);
							break;
						}
					}
				}
			}

			public IEventListener<TEventListener>[] GetListeners(GameObject[] activeObjs)
			{
				List<EventListenerInfo> allListeners = new List<EventListenerInfo>();
				allListeners.AddRange(_alwaysReceiveEvents);

				foreach (GameObject obj in activeObjs)
				{
					List<EventListenerInfo> gameObjlisteners;
					if (_gameObjToListenersMap.TryGetValue(obj, out gameObjlisteners))
					{
						allListeners.AddRange(gameObjlisteners);
					}
				}

				return allListeners.Select(info => info.Listener).ToArray();
			}

			private class EventListenerInfo
			{
				public IEventListener<TEventListener> Listener;
				public GameObject Owner;
				public ReceiveEventStates ReceiveEventState;
			}
		}
	}
}
