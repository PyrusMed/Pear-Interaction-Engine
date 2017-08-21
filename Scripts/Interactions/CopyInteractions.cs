using UnityEngine;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Copies interactions from the current object to the supplied objects
	/// </summary>
	public class CopyInteractions: MonoBehaviour
	{
		[Tooltip("Objects that interactions will be copied to")]
		public GameObject[] CopyTo;

		[Tooltip("Objects that the interactions will be copied from")]
		public GameObject[] CopyFrom;

		private void Awake()
		{
			if (CopyTo != null)
			{
				// Copy all interactions from the current object to the specified object
				foreach (GameObject copyTo in CopyTo)
					Interaction.CopyAll(gameObject, copyTo);
			}

			if(CopyFrom != null)
			{
				// Copy all interactions from the specified objects to this game object
				foreach (GameObject copyFrom in CopyFrom)
					Interaction.CopyAll(copyFrom, gameObject);
			}
		}
	}
}
