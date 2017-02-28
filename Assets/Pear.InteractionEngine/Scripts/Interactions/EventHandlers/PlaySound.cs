using Pear.InteractionEngine.Properties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Pear.InteractionEngine.Interactables.Behaviors
{
	public class PlaySound : PlaySoundBase<bool>
	{
		protected override void PlaySoundHandler(bool oldValue, bool newValue)
		{
			if (newValue)
				TryToPlayStartSound();
			else
				TryToPlayEndSound();
		}
	}
}
