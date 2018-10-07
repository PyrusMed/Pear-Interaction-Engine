using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Manipulating objects, such as resizing and moving, can be tricky when you have multiple scripts trying the modify the same thing.
	/// This class creates an anchor element which is used to help these manipulations
	/// </summary>
	public class ObjectWithAnchor : MonoBehaviour
    {
		/// <summary>
		/// Used to move and manipulate this object
		/// </summary>
        public Anchor AnchorElement;

        void Awake()
        {
			if (AnchorElement != null)
				return;

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