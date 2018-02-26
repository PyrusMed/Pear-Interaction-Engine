using Pear.InteractionEngine.Controllers;
using System;
using UnityEngine.Events;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Encapsulates data when event value changes
	/// </summary>
	/// <typeparam name="T">Type of event value</typeparam>
	public struct EventArgs<T>
	{
		/// <summary>
		/// Event source
		/// </summary>
		public Controller Source;

		/// <summary>
		/// Event's old value
		/// </summary>
		public T OldValue;

		/// <summary>
		/// Event's new value
		/// </summary>
		public T NewValue;
	}

	[Serializable]
	public class EventArgsUnityEvent<T> : UnityEvent<EventArgs<T>> { }
}
