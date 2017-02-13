using Pear.Core.Controllers;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace Pear.Core.Controllers.Behaviors
{
    /// <summary>
    /// Drag to rotate the controller's active object
    /// </summary>
    public class DragToZoom : DragBehavior
    {

        /// <summary>
        /// Zoom in/out based on the drag factor
        /// </summary>
        /// <param name="actionFactor">relative position * MaxSpeed</param>
        protected override void PerformAction(Vector3 actionFactor)
        {
            Controller.ActiveObject.transform.localScale += Vector3.one * Vector3.Distance(Vector3.zero, actionFactor / 100);
        }
    }
}
