namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	/// <summary>
	/// Plays sound when an integer property changes
	/// NOTE:
	///		We could do something more interesting with this, like adjust the volume
	///		or speed based on the property's value. Hmmmmmmm.
	/// </summary>
	public class PlaySoundAnalog :  PlaySoundBase<int>
	{
		protected override void PlaySoundHandler(int oldValue, int newValue)
		{
			if (oldValue == 0 && newValue != 0)
				TryToPlayStartSound();
			else if(oldValue != 0 && newValue == 0)
				TryToPlayEndSound();
		}
	}
}
