using UnityEngine;

namespace Pear.Core.Controllers.Behaviors
{
    /// <summary>
    /// Drag to rotate the controller's active object
    /// </summary>
    public class DragToRotate : DragBehavior
    {

        /// <summary>
        /// Rotate this object based on the drag factos
        /// </summary>
        /// <param name="actionFactor">relative position * MaxSpeed</param>
        protected override void PerformAction(Vector3 actionFactor)
        {
            Controller.ActiveObject.transform.Rotate(new Vector3(actionFactor.y, -actionFactor.x, 0), Space.World);
        }
    }
}