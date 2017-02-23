using Pear.InteractionEngine.Interactables;
using Pear.InteractionEngine.Properties;
using UnityEngine;

namespace Pear.InteractionEngine.Controllers.Behaviors
{
    /// <summary>
    /// Update the interactable object's state when we hover over it
    /// </summary>
    public class HoverOnGaze : ControllerBehavior<Controller>
    {
        [Tooltip("Name of the hover property to look for in hoverable objects")]
        public string HoverPropertyName = "pie.hover";

        public GameObject HoveredObject
        {
            get { return _lasthHoverProperty != null ? _lasthHoverProperty.gameObject : null; }
        }

        private BoolGameObjectProperty _lasthHoverProperty;

        void Update()
        {
            RaycastHit hitInfo;
			BoolGameObjectProperty newHoverProperty = null;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 1000))
                newHoverProperty = hitInfo.transform.gameObject.GetProperty<BoolGameObjectProperty, bool>(HoverPropertyName);

            // If hovering changed...
            if (newHoverProperty != _lasthHoverProperty)
            {
                // If there's an old object stop hovering over it
                if (_lasthHoverProperty != null)
                {
                    _lasthHoverProperty.Value = false;
                    _lasthHoverProperty = null;
                }

                // If there's a new object hover over it
                if (newHoverProperty != null)
                {
                    newHoverProperty.Value = true;
                    _lasthHoverProperty = newHoverProperty;
                }
            }
        }
    }
}