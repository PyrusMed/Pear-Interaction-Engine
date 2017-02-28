using Pear.InteractionEngine.Properties;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pear.InteractionEngine.Interactables
{
    public class ObjectWithAnchor : MonoBehaviour
    {
		/// <summary>
		/// Used to move and manipulate this object
		/// </summary>
		[HideInInspector]
        public Anchor AnchorElement;

        void Awake()
        {
            // Create the anchor element
            GameObject anchor = new GameObject("Anchor");
            AnchorElement = anchor.AddComponent<Anchor>();
            AnchorElement.Child = this;
            AnchorElement.transform.position = transform.position;
            anchor.transform.SetParent(transform.parent, true);
            transform.SetParent(AnchorElement.transform, true);
        }
    }
}