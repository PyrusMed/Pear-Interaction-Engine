namespace Pear.InteractionEngine.Converters
{
	/// <summary>
	/// Interface for converting from one type to another.
	/// Used to convert IEvent values to values IEventListener can understand
	/// </summary>
	/// <typeparam name="TConvertFrom">Type to convert from</typeparam>
	/// <typeparam name="TConvertTo">Type to convert to</typeparam>
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

