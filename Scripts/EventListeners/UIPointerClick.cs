using UnityEngine;
using Pear.InteractionEngine.Events;
using Pear.InteractionEngine.Interactions;

namespace Pear.InteractionEngine.EventListeners
{
	/// <summary>
	/// Clicks the pointer based on change in event
	/// </summary>
	public class UIPointerClick : MonoBehaviour, IEventListener<bool>
	{
		[Tooltip("Pointer to click when event value is true")]
		public UIPointer Pointer;

		/// <summary>
		/// When the value changes, click the pointer
		/// </summary>
		/// <param name="args"></param>
		public void ValueChanged(EventArgs<bool> args)
		{
			Pointer.Click = args.NewValue;
		}
	}
}
