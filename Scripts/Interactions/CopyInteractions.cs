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

		private void Awake()
		{
			if (CopyTo != null)
			{
				//Copy alkl interactions from the current object to the specified object
				foreach (GameObject copyTo in CopyTo)
					Interaction.CopyAll(gameObject, copyTo);
			}
		}
	}
}
