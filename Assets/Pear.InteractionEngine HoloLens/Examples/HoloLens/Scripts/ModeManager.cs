using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pear.InteractionEngine.Interactions;
using System.Linq;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Manages the current mode
	/// </summary>
	public class ModeManager : Singleton<ModeManager>
	{
		[Tooltip("The mode to start in")]
		public Modes DefaultMode = Modes.Rotate;

		// Mode changed event handling
		public delegate void ModeChangedHandler(Modes mode);
		public event ModeChangedHandler ModeChangedEvent;

		// The current mode
		private Modes _mode;
		public Modes Mode
		{
			get { return _mode; }
			set
			{
				SetMode(value);
			}
		}

		void Start()
		{
			// Listen for each command
			foreach(Modes mode in Enum.GetValues(typeof(Modes)).Cast<Modes>())
			{
				VoiceCommandManager.Instance.ListenForCommand(
					mode.ToString(),
					args => Mode = mode);
			}

			SetMode(DefaultMode, forceChangedEvent: true);
		}

		/// <summary>
		/// Set the mode
		/// </summary>
		/// <param name="newMode">new mode to set mode to</param>
		/// <param name="forceChangedEvent">Should we force the change event?</param>
		private void SetMode(Modes newMode, bool forceChangedEvent = false)
		{
			Modes oldMode = _mode;
			_mode = newMode;

			if (ModeChangedEvent != null && (forceChangedEvent || oldMode != newMode))
				ModeChangedEvent(_mode);
		}
	}
}
