using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public class RaycastHitToBool : MonoBehaviour, IPropertyConverter<RaycastHit?, bool>
	{
		public bool Convert(RaycastHit? convertFrom)
		{
			return convertFrom.HasValue ?
				convertFrom.Value.transform.gameObject == gameObject :
				false;
		}
	}
}
