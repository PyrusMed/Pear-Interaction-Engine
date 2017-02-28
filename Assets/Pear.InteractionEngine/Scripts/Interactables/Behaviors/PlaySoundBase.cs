using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
	public abstract class PlaySoundBase<T> : MonoBehaviour, IGameObjectPropertyEventHandler<T>
	{
		public AudioSource StartSound;
		public AudioSource EndSound;

		public void RegisterProperty(GameObjectProperty<T> property)
		{
			property.OnChange += PlaySoundHandler;
		}

		public void UnregisterProperty(GameObjectProperty<T> property)
		{
			property.OnChange -= PlaySoundHandler;
		}

		protected void TryToPlayStartSound()
		{
			if (StartSound != null)
				StartSound.Play();
		}

		protected void TryToPlayEndSound()
		{
			if (EndSound != null)
				EndSound.Play();
		}

		protected abstract void PlaySoundHandler(T oldValue, T newValue);

		/// <summary>
		/// Create an audio source from the specified audio clip
		/// </summary>
		/// <param name="clip">Clip to create source from</param>
		/// <returns>Audio source</returns>
		private AudioSource CreateAudioSource(AudioClip clip)
		{
			if (clip == null)
				return null;

			AudioSource source = gameObject.AddComponent<AudioSource>();
			source.clip = clip;
			source.playOnAwake = false;
			return source;
		}
	}
}
