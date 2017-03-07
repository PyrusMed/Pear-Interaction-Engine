using Leap.Unity;

namespace Pear.InteractionEngine.Interactions.EventHandlers
{
	public class PlaySoundLeap : PlaySoundBase<IHandModel>
	{
		/// <summary>
		/// Handles playing sounds based on the change in a property's value
		/// </summary>
		protected override void PlaySoundHandler(IHandModel oldValue, IHandModel newValue)
		{
			if (newValue)
				TryToPlayStartSound();
			else
				TryToPlayEndSound();
		}
	}
}
