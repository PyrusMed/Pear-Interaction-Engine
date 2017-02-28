using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
    /// <summary>
    /// Drag to rotate the controller's active object
    /// </summary>
    public class DragToZoom : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
    {
        [Tooltip("Zoom speed")]
        public float ZoomSpeed = 3f;

        List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        void Update()
        {
            _properties.ForEach(p =>
            {
                p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
                    .AnchorElement
                    .transform
                    .localScale += Vector3.one * Vector3.Distance(Vector3.zero, p.Value / 100) * ZoomSpeed * Time.deltaTime;
            });
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
