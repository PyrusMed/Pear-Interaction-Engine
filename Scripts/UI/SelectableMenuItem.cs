using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// Enables navigation between menu items
	/// </summary>
	[RequireComponent(typeof(Selectable))]
	public class SelectableMenuItem : MonoBehaviour
	{
		[Tooltip("Item to be selected on up")]
		public SelectableMenuItem OnUp;

		[Tooltip("Item to be selected on down")]
		public SelectableMenuItem OnDown;

		[Tooltip("Item to be selected on left")]
		public SelectableMenuItem OnLeft;

		[Tooltip("Item to be selected on right")]
		public SelectableMenuItem OnRight;

		/// <summary>
		/// Selects the selectable
		/// </summary>
		public void Select()
		{
			GetComponent<Selectable>().Select();
		}
	}
}
