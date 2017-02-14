using UnityEngine;

namespace Pear.InteractionEngine.Interactables
{
    /// <summary>
    /// Parent to Interactable objects that makes interacting with elements, such as rotating and zooming, much easier
    /// </summary>
    public class Anchor : MonoBehaviour
    {

        // Interactable child element
        public InteractableObject Interactable;
    }
}