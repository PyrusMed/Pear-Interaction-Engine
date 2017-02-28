using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
    /// <summary>
    /// Parent to Interactable objects that makes interacting with elements, such as rotating and zooming, much easier
    /// </summary>
    public class Anchor : MonoBehaviour
    {

        // Interactable child element
        public ObjectWithAnchor Child;
    }
}