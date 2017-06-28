using Pear.InteractionEngine.Events;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Listens to an event's value change
	/// </summary>
	/// <typeparam name="T">Type of value</typeparam>
	public interface IEventListener<T>
	{
		void ValueChanged(EventArgs<T> args);
	}
}