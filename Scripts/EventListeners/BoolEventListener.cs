using Pear.InteractionEngine.Events;
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
		public UnityEvent TrueEvent;

		[Tooltip("Fired when the value changes from true to false")]
		public UnityEvent FalseEvent; 

		public void ValueChanged(EventArgs<bool> args)
		{
			if (args.NewValue)
				TrueEvent.Invoke();
			else
				FalseEvent.Invoke();
		}
	}
}
