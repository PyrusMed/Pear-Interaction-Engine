using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Examples
{
	/// <summary>
	/// Display text for the current mode
	/// </summary>
	public class ModeText : MonoBehaviour
	{
		// Use this for initialization
		void Start()
		{
			TextMesh textMesh = GetComponent<TextMesh>();

			// When the mode changes show text for that mode
			ModeManager.Instance.ModeChangedEvent += mode => textMesh.text = "Mode: " + mode;
		}
	}
}