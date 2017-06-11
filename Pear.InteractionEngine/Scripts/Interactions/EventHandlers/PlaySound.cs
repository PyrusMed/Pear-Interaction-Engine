namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Plays a sound when a boolean property changes
	/// </summary>
	public class PlaySound : PlaySoundBase<bool>
	{
		/// <summary>
		/// Play start or end sound when a boolean property changes
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected override void PlaySoundHandler(bool oldValue, bool newValue)
		{
			if (newValue)
				TryToPlayStartSound();
			else
				TryToPlayEndSound();
		}
	}
}
