using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Interactions.Events;
using System.Collections.Generic;

namespace Pear.InteractionEngine.Examples
{
    public class HoloLensControllerExample : HoloLensController
    {
		// Tap to select control
        public TapToSelect TapToSelect;

		// Pinch to drag controls
		public PinchAndDrag Rotator;
		public PinchAndDrag Resizer;
		public PinchAndDrag Mover;

		void Awake()
        {
            TapToSelect.SelectedEvent += go => ActiveObject = go;

			// Maps a control to a mode
			Dictionary<Modes, PinchAndDrag> _modeControlMap = new Dictionary<Modes, PinchAndDrag>()
			{
				{ Modes.Rotate, Rotator },
				{ Modes.Zoom, Resizer },
				{ Modes.Move, Mover },
			};

			// When the mode changes enable to right control
			ModeManager.Instance.ModeChangedEvent += mode =>
			{
				foreach(KeyValuePair<Modes, PinchAndDrag> modeControl in _modeControlMap)
					modeControl.Value.enabled = modeControl.Key == mode;
			};
        }
    }
}