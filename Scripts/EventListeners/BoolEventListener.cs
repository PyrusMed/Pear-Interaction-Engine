using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Fires events based on the event listener input
	/// </summary>
	public class BoolEventListener : MonoBehaviour, IEventListener<bool>
	{
		[Tooltip("Fired when the value changes from false to true")]
		public BoolEventArgsUnityEvent TrueEvent;

		[Tooltip("Fired when the value changes from true to false")]
		public BoolEventArgsUnityEvent FalseEvent;

		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue)
				TrueEvent.Invoke(args);
			else
				FalseEvent.Invoke(args);
		}
	}

	[Serializable]
	public class BoolEventArgsUnityEvent : EventArgsUnityEvent<bool> { }
}
