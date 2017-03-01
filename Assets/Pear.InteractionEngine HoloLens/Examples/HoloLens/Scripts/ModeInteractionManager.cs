using Pear.InteractionEngine.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Enables interactions when in that mode. Otherwise, interactions are disabled
	/// </summary>
	public class ModeInteractionManager : MonoBehaviour
	{
		// Links an interaction to a specific mode
		public ModeInteraction[] ModeInteractions;

		// Use this for initialization
		void Awake()
		{
			// When the mode changes make sure only interactions linked to that mode are enabled.
			// All other interactions should be disabled
			ModeManager.Instance.ModeChangedEvent += newMode =>
			{
				if (ModeInteractions != null)
				{
					foreach (ModeInteraction modeInteraction in ModeInteractions)
					{
						if (modeInteraction.Interaction == null)
							Debug.LogError("Interaction should not be null");
						else
							modeInteraction.Interaction.enabled = modeInteraction.Mode == newMode;
					}
				}
			};
		}
	}

	[Serializable]
	public class ModeInteraction
	{
		public Modes Mode;
		public Interaction Interaction;
	}
}
