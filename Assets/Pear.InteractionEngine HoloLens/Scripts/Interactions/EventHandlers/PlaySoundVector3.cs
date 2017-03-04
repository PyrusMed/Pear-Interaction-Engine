using System;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Plays sounds based on vector3 input. When input starts (0 -> non-zero)
	/// this class plays the start sound. When input ends (non-zero -> zero) it plays
	/// the end sound
	/// </summary>
	public class PlaySoundVector3 : PlaySoundBase<Vector3>
	{
		protected override void PlaySoundHandler(Vector3 oldValue, Vector3 newValue)
		{
			if (oldValue == Vector3.zero)
				TryToPlayStartSound();
			else if (newValue == Vector3.zero)
				TryToPlayEndSound();
		}
	}
}
