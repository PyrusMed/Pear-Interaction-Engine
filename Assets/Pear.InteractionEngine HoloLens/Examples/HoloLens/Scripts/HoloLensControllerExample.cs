using Pear.InteractionEngine.Controllers;
using Pear.InteractionEngine.Controllers.Behaviors;

namespace Pear.InteractionEngine.Examples
{
    public class HoloLensControllerExample : HoloLensController
    {
        public TapToSelect TapToSelect;
        public DragToRotate DragToRotate;
        public DragToZoom DragToZoom;

        void Awake()
        {
            TapToSelect.SelectedEvent += go => ActiveObject = go;
        }

        public void EnterRotateMode()
        {
            DragToRotate.enabled = true;
            DragToZoom.enabled = false;
        }

        public void EnterZoomMode()
        {
            DragToZoom.enabled = true;
            DragToRotate.enabled = false;
        }
    }
}