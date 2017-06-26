using Pear.InteractionEngine.Properties;

namespace Pear.InteractionEngine.Events
{
	public interface IEvent<T>
	{
		Property<T> Event { get; set; }
	}
}
