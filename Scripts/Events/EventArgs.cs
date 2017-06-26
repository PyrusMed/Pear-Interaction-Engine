using Pear.InteractionEngine.Controllers;

namespace Pear.InteractionEngine.Events
{
	public struct EventArgs<T>
	{
		// Event source
		public Controller Source;

		// Event's old value
		public T OldValue;

		// Event's new value
		public T NewValue;
	}
}
