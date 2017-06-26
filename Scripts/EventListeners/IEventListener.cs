using Pear.InteractionEngine.Events;

namespace Pear.InteractionEngine.EventListeners
{
	public interface IEventListener<T>
	{
		void ValueChanged(EventArgs<T> args);
	}
}