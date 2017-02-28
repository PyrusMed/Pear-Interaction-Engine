using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Base class for classes that play a sound based on the change in value of a property
	/// </summary>
	/// <typeparam name="T">Type of value that changes</typeparam>
	public abstract class PlaySoundBase<T> : MonoBehaviour, IGameObjectPropertyEventHandler<T>
	{
		[Tooltip("Sound that's played when the event starts")]
		public AudioSource StartSound;

		[Tooltip("Sound that's played when the event ends")]
		public AudioSource EndSound;

		/// <summary>
		/// Register the property value change handler
		/// </summary>
		/// <param name="property">property to listen to</param>
		public void RegisterProperty(GameObjectProperty<T> property)
		{
			property.ChangeEvent += PlaySoundHandler;
		}

		/// <summary>
		/// Unregister the property value change handler
		/// </summary>
		/// <param name="property">Property to stop listening to</param>
		public void UnregisterProperty(GameObjectProperty<T> property)
		{
			property.ChangeEvent -= PlaySoundHandler;
		}

		/// <summary>
		/// Play the start sound if it's not null
		/// </summary>
		protected void TryToPlayStartSound()
		{
			if (StartSound != null)
				StartSound.Play();
		}

		/// <summary>
		/// Play the end sound if it's not null
		/// </summary>
		protected void TryToPlayEndSound()
		{
			if (EndSound != null)
				EndSound.Play();
		}

		/// <summary>
		/// Handles playing sounds based on the change in a property's value
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected abstract void PlaySoundHandler(T oldValue, T newValue);
	}
}
