using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Interactions.Events;

namespace Pear.InteractionEngine.Examples
{
    public class HoloLensControllerExample : HoloLensController
    {
        public TapToSelect TapToSelect;

        void Awake()
        {
            TapToSelect.SelectedEvent += go => ActiveObject = go;
        }
    }
}