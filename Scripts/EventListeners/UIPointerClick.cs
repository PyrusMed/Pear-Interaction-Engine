using Pear.InteractionEngine.EventListeners;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pear.InteractionEngine.Events;
using System;
using Pear.InteractionEngine.Interactions;

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
