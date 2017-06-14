using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pear.InteractionEngine.Properties.Converters
{
	public interface IPropertyConverter<TConvertFrom, TConvertTo>
	{
		/// <summary>
		/// Converts from a type, to a type
		/// </summary>
		/// <param name="convertFrom">Type to convert from</param>
		/// <returns>Converted value</returns>
		TConvertTo Convert(TConvertFrom convertFrom);
	}
}

