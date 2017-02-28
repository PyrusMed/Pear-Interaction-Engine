using Pear.InteractionEngine.Properties;
using UnityEngine;
using System.Collections.Generic;
using Pear.InteractionEngine.Utils;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
    /// <summary>
    /// Drag to rotate the controller's active object
    /// </summary>
    public class DragToRotate : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
    {
        [Tooltip("Rotation speed")]
        public float RotateSpeed = 3f;

        List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        void Update()
        {
            _properties.ForEach(p =>
            {
                p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
                    .AnchorElement
                    .transform
                    .Rotate(new Vector3(p.Value.y, -p.Value.x, 0) * RotateSpeed * Time.deltaTime, Space.World);
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