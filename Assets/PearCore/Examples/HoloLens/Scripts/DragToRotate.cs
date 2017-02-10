using Pear.Core.Controllers;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

/// <summary>
/// Drag to rotate the controller's active object
/// </summary>
public class DragToRotate : DragBehavior {

    /// <summary>
    /// Perform the action based on the given factor
    /// </summary>
    /// <param name="actionFactor">relative position * MaxSpeed</param>
    protected override void PerformAction(Vector3 actionFactor)
    {
        Controller.ActiveObject.transform.Rotate(new Vector3(actionFactor.y, -actionFactor.x, 0), Space.World);
    }
}
