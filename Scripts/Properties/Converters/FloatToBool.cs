using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class FloatToBool : MonoBehaviour, IPropertyConverter<float, bool>
	{
		public float Threshold = 0.5f;

		public bool GreaterThanOrEqualToThreshold = true;

		public bool Convert(float convertFrom)
		{
			return GreaterThanOrEqualToThreshold ? 
				convertFrom >= Threshold :
				convertFrom <= Threshold;
		}
	}
}
