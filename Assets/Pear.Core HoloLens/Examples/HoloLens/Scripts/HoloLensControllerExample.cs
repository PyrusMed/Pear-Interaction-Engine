using Pear.Core.Controllers;
using Pear.Core.Controllers.Behaviors;

public class HoloLensControllerExample : HoloLensController {

    public DragToRotate DragToRotate;
    public DragToZoom DragToZoom;
     
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
