namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Classes that implement this interface modify properties that are later handled by an IGameOBjectPropertyEventHandler.
	/// </summary>
	/// <typeparam name="T">Type of property</typeparam>
	public interface IGameObjectPropertyEvent<T> : IGameObjectPropertyManager<T>
	{
	}
}