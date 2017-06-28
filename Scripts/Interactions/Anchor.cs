using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
    /// <summary>
    /// Parent object to ObjectWithAnchor. This element makes interacting, such as rotating and zooming, much easier
    /// </summary>
    public class Anchor : MonoBehaviour
    {
        // Child element
        public ObjectWithAnchor Child;
    }
}