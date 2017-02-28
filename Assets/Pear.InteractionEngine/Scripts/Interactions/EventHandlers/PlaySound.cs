namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	public class PlaySound : PlaySoundBase<bool>
	{
		protected override void PlaySoundHandler(bool oldValue, bool newValue)
		{
			if (newValue)
				TryToPlayStartSound();
			else
				TryToPlayEndSound();
		}
	}
}
