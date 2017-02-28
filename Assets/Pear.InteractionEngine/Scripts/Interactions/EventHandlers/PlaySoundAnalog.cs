namespace Pear.InteractionEngine.Interactions.EventHandlers
{
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
