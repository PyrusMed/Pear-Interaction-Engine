using Pear.Core.Controllers;
using Pear.Core.Interactables;
using UnityEngine;

/// <summary>
/// Update the interactable object's state when we hover over it
/// </summary>
public class HoverOnGaze : ControllerBehavior<Controller> {

    public InteractableObject HoveredObject { get; private set; }

    // Update is called once per frame
    void Update() {
        RaycastHit hitInfo;
        InteractableObject interactable = null;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
            interactable = hitInfo.transform.gameObject.GetComponent<InteractableObject>();

        if (interactable != HoveredObject)
        {
            if (HoveredObject != null) {
                HoveredObject.Hovering.Remove(Controller);
                HoveredObject = null;
            }

            if(interactable != null)
            {
                interactable.Hovering.Add(Controller);
                HoveredObject = interactable;
            }
        }
    }
}
