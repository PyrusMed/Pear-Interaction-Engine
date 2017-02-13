using UnityEngine;
using Leap;
using Leap.Unity;

/// <summary>
/// Places this object on the tip of the index finger at all times
/// </summary>
public class FollowIndexFinger : MonoBehaviour {

	// Hand
	public HandModel Hand;
	
	// Place object on index finger
	void Update () {
		transform.position = GetTipPosition (1);
	}

	/// <summary>
	/// Gets the tip position of a finger.
	/// </summary>
	/// <returns>The tip position.</returns>
	/// <param name="fingerIndex">Finger index.</param>
	Vector3 GetTipPosition(int fingerIndex) {
		Hand leap_hand = Hand.GetLeapHand();
		Vector tip = leap_hand.Fingers[fingerIndex].TipPosition;
		return new Vector3 (tip.x, tip.y, tip.z);
	}
}
