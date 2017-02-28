using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
    /// <summary>
    /// Zoom in/out based on the change in property value
    /// </summary>
    public class ZoomVector3 : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
    {
        [Tooltip("Zoom speed")]
        public float ZoomSpeed = 3f;

        List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        void Update()
        {
            foreach (GameObjectProperty<Vector3> property in _properties)
            {
                // Better to resize the anchor element than the original object since there could be other
                // event handlers applying manipulations
                Anchor anchor = property.Owner.transform.GetOrAddComponent<ObjectWithAnchor>().AnchorElement;
                float currentScale = anchor.transform.localScale.x;
                float scaleAmount = property.Value.magnitude * Time.deltaTime;
                float newScale = currentScale * (1 + scaleAmount);

                // Apply the new scale
                anchor.transform.localScale = Vector3.one * newScale;
            }
        }

        public void RegisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Add(property);
        }

        public void UnregisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Add(property);
        }
    }
}
