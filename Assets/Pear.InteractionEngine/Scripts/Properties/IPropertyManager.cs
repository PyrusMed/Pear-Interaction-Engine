namespace Pear.InteractionEngine.Properties
{
	/// <summary>
	/// Classes that implement this interface interact with GameObjectProperties to create interactions
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGameObjectPropertyManager<T>
	{
		void RegisterProperty(GameObjectProperty<T> property);
		void UnregisterProperty(GameObjectProperty<T> property);
	}
}
