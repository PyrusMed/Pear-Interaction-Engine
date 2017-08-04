using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// Highlights text with a specified color
	/// </summary>
	public class HighlightText : AHighlight<Text, Color>
	{
		protected override Color HighlightProperty
		{
			get { return ObjectToHighlight.color; }
			set { ObjectToHighlight.color = value; }
		}
	}
}
