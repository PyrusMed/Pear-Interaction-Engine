using Pear.InteractionEngine.Interactables;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
    /// <summary>
    /// Update the interactable object's state when we hover over it
    /// </summary>
    public class HoverOnGaze : ControllerBehavior<Controller>
    {
        public InteractableObject HoveredObject { get; private set; }

        void Update()
        {
            RaycastHit hitInfo;
            InteractableObject interactable = null;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
                interactable = hitInfo.transform.gameObject.GetComponent<InteractableObject>();

            // If hovering changed...
            if (interactable != HoveredObject)
            {
                // If there's an old object stop hovering over it
                if (HoveredObject != null)
                {
                    HoveredObject.Hovering.Remove(Controller);
                    HoveredObject = null;
                }

                // If there's a new object hover over it
                if (interactable != null)
                {
                    interactable.Hovering.Add(Controller);
                    HoveredObject = interactable;
                }
            }
        }
    }
}