using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.UI
{
	/// <summary>
	/// Highlights a game object with a specified material
	/// </summary>
	public class HighlightMaterial : AHighlight<GameObject, Material>
	{
		protected override Material HighlightProperty
		{
			get { return ObjectToHighlight.GetComponent<Renderer>().material; }
			set { ObjectToHighlight.GetComponent<Renderer>().material = value; }
		}
	}
}
