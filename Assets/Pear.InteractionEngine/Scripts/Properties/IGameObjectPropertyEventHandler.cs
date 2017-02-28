namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Classes that implement this interface handle changes to properties.
	/// For example, an implementer of this class might play a sound when a property changes (see PlaySound.cs)
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGameObjectPropertyEventHandler<T> : IGameObjectPropertyManager<T>
	{
	}
}