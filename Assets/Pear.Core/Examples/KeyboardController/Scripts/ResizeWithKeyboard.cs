using Pear.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeWithKeyboard : ControllerBehavior<Controller> {

    [Tooltip("Key used to make the object bigger")]
    public KeyCode MakeSmallerKey = KeyCode.Alpha1;

    [Tooltip("Key used to make the object smaller")]
    public KeyCode MakeBiggerKey = KeyCode.Alpha2;

    [Tooltip("The percent per second in which the object changes while scaling")]
    public float ScalePercentage = 0.1f;

    // Tracks whether we resized during the last frame
    private bool _lastResizedState = false;
	
	// Update is called once per frame
	void Update () {
        if (Controller.ActiveObject == null)
            return;

        bool resized = true;
        if (Input.GetKey(MakeSmallerKey))
            Resize(increase: false);
        else if (Input.GetKey(MakeBiggerKey))
            Resize(increase: true);
        else
            resized = false;

        // If we started or stopped resizing update the object's state
        if(resized != _lastResizedState)
        {
            if (resized)
                Controller.ActiveObject.Resizing.Add(Controller);
            else
                Controller.ActiveObject.Resizing.Remove(Controller);
        }

        _lastResizedState = resized;
    }

    /// <summary>
    /// Resize the active object
    /// </summary>
    /// <param name="increase"></param>
    void Resize(bool increase)
    {
        int direction = increase ? 1 : -1;
        float scaleAmount = ScalePercentage * Time.deltaTime;
        float currentScale = Controller.ActiveObject.AnchorElement.transform.localScale.x;
        float newScale = currentScale * (1 + scaleAmount * direction);

        // Apply the new scale
        Controller.ActiveObject.AnchorElement.transform.localScale = Vector3.one * newScale;
    }
}
