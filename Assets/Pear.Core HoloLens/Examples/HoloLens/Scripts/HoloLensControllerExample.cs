using Pear.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloLensControllerExample : Controller {

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
