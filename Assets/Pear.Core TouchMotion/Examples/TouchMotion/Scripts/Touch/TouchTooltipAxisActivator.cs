using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates a tooltip based on user interaction with a trigger button
/// </summary>
public class TouchTooltipAxisActivator : TouchTooltipActivator
{
	[Tooltip("Axis to listen to to show/hide the tooltip")]
	public OVRInput.Axis1D Axis1DListener;

	[Tooltip("If pressure on button exceeds this threshold the tooltip will display")]
	public float LowerBound = 0.02f;

	[Tooltip("If pressure on button exceeds this threshold the tooltip will hide")]
	public float UpperBound = 0.9f;

	/// <summary>
	/// Start the timer if the user doesn't fully press the button
	/// </summary>
	/// <returns>True if user doesn't full press the button. False otherwise.</returns>
	protected override bool ShouldStartTimer()
	{
		return IsInShowZone();
    }

	/// <summary>
	/// Stop the timer if the user fully presses the button or releases the button completely.
	/// </summary>
	/// <returns>True if user is not in the show zone. False otherwise.</returns>
	protected override bool ShouldStopTimer()
	{
		return !IsInShowZone();
    }

	/// <summary>
	/// Tells whether the user is between the bounds
	/// </summary>
	/// <returns>True if the user is pressing the button just enough to be between the given bounds. False otherwise.</returns>
	private bool IsInShowZone()
	{
		float val = OVRInput.Get(Axis1DListener, _touchController.OVRController);
		return val > LowerBound && val < UpperBound;
	}
}
