using Pear.InteractionEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	public class EventMap : Singleton<EventMap>
	{
		public MappedEvent[] Map;

		public Dictionary<string, Type> EventNameToType
		{
			get { return CreateEventNameToTypeDict(); }
		}

		private Dictionary<string, Type> CreateEventNameToTypeDict()
		{
			Dictionary<string, Type> eventNameToType = new Dictionary<string, Type>();
			foreach (MappedEvent mappedEvent in Map)
			{
				eventNameToType[mappedEvent.Name] = GetType(mappedEvent.EventHandlerType);
			}

			return eventNameToType;
		}

		private Type GetType(EventHandlerTypes eventHandlerType)
		{
			switch(eventHandlerType)
			{
				case EventHandlerTypes.Bool:
					return typeof(bool);
				case EventHandlerTypes.Float:
					return typeof(float);
				case EventHandlerTypes.GameObject:
					return typeof(GameObject);
				case EventHandlerTypes.Int:
					return typeof(int);
				case EventHandlerTypes.RaycastHitNullable:
					return typeof(RaycastHit?);
				case EventHandlerTypes.Vector2:
					return typeof(Vector2);
				case EventHandlerTypes.Vector3:
					return typeof(Vector3);
				case EventHandlerTypes.Vector4:
					return typeof(Vector4);
				default:
					throw new ArgumentOutOfRangeException("An does not exist with value: " + eventHandlerType);
			}
		}
	}

	[Serializable]
	public class MappedEvent
	{
		public string Name;
		public EventHandlerTypes EventHandlerType;
	}

	[Serializable]
	public enum EventHandlerTypes
	{
		Bool,
		Float,
		Int,
		RaycastHitNullable,
		Vector2,
		Vector3,
		Vector4,
		GameObject,
	}
}
