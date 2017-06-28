using Pear.InteractionEngine.Properties;

namespace Pear.InteractionEngine.Events
{
	/// <summary>
	/// Interface representing an event. Implementers set Event.Value to update the event's
	/// value that's sent to classes that implement IEventListener
	/// </summary>
	/// <typeparam name="T">Type of event value</typeparam>
	public interface IEvent<T>
	{
		/// <summary>
		/// Property associated with this event
		/// </summary>
		Property<T> Event { get; set; }
	}
}
