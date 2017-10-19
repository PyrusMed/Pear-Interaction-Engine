using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Copies interactions from the current object to the supplied objects
	/// </summary>
	public class CopyNamedEventListeners : MonoBehaviour
	{
		[Tooltip("Objects that interactions will be copied to")]
		public GameObject[] CopyTo;

		[Tooltip("Objects that the interactions will be copied from")]
		public GameObject[] CopyFrom;

		private void Awake()
		{
			if (CopyTo != null)
			{
				// Copy all listeners from the current object to the specified object
				foreach (GameObject copyTo in CopyTo)
					NamedEventListener.CopyAll(gameObject, copyTo);
			}

			if(CopyFrom != null)
			{
				// Copy all listeners from the specified objects to this game object
				foreach (GameObject copyFrom in CopyFrom)
					NamedEventListener.CopyAll(copyFrom, gameObject);
			}
		}
	}
}
