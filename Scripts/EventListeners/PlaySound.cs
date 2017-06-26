using Pear.InteractionEngine.Events;
using UnityEngine;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Plays a sound based on the boolean event value
	/// </summary>
	public class PlaySound : MonoBehaviour, IEventListener<bool> 
	{
		[Tooltip("Sound played when the event value is true")]
		public AudioSource TrueSound;

		[Tooltip("Sound played when the event value is false")]
		public AudioSource FalseSound;

		/// <summary>
		/// Attempts to play a sound based on the event
		/// </summary>
		/// <param name="args">Event args</param>
		public void ValueChanged(EventArgs<bool> args)
		{
			AudioSource sound = args.NewValue ? TrueSound : FalseSound;
			TryAndPlay(sound);
		}

		/// <summary>
		/// Attempts to play the given sound.
		/// </summary>
		/// <param name="sound">Sound to play</param>
		private void TryAndPlay(AudioSource sound)
		{
			if (sound != null)
				sound.Play();
		}
	}
}
