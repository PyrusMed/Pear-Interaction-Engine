using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
	public class SoundPlayer : MonoBehaviour, IGameObjectPropertyAction<bool>
	{
		public AudioClip StartSound;
		public AudioClip EndSound;

		private AudioSource _startSource;
		private AudioSource _endSource;

		void Start()
		{
			_startSource = CreateAudioSource(StartSound);
			_endSource = CreateAudioSource(EndSound);
		}

		public void RegisterProperty(GameObjectProperty<bool> property)
		{
			property.OnChange += PlaySoundHandler;
		}

		public void UnregisterProperty(GameObjectProperty<bool> property)
		{
			property.OnChange -= PlaySoundHandler;
		}

		private void PlaySoundHandler(bool oldValue, bool newValue)
		{
			if (newValue)
			{
				if (_startSource != null)
					_startSource.Play();
			}
			else
			{
				if (_endSource != null)
					_endSource.Play();
			}
		}

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
