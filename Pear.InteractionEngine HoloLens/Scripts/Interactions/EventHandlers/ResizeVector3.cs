using Pear.InteractionEngine.Properties;
using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
    /// <summary>
    /// Resize based on the change in property value
    /// </summary>
    public class ResizeVector3 : MonoBehaviour, IGameObjectPropertyEventHandler<Vector3>
    {
        [Tooltip("Resize speed")]
        public float ResizeSpeed = 3f;

        List<GameObjectProperty<Vector3>> _properties = new List<GameObjectProperty<Vector3>>();

        void Update()
        {
            _properties.ForEach(p =>
            {
                p.Owner.transform.GetOrAddComponent<ObjectWithAnchor>()
                    .AnchorElement
                    .transform
                    .localScale += Vector3.one * -p.Value.x * ResizeSpeed * Time.deltaTime;
            });
        }

        public void RegisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Add(property);
        }

        public void UnregisterProperty(GameObjectProperty<Vector3> property)
        {
            _properties.Remove(property);
        }
    }
}
