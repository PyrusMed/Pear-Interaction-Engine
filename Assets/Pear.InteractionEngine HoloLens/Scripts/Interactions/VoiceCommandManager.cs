using Pear.InteractionEngine.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;

namespace Pear.InteractionEngine.Interactions
{
	/// <summary>
	/// Manages voice commands. Anyone who need to listen to a voice command can register
	/// a callback function here that will be executed when the user says the specified command
	/// </summary>
	public class VoiceCommandManager : Singleton<VoiceCommandManager>
	{
		// Recognizes voice commands
		private KeywordRecognizer _recognizer;

		// Maps a voice command to a function
		private Dictionary<string, List<KeywordAction>> _commandToFunctionMap = new Dictionary<string, List<KeywordAction>>();

		/// <summary>
		/// Adds a listener to a particular voice command
		/// </summary>
		/// <param name="command">command to listen for</param>
		/// <param name="callback">callback executed when command is said</param>
		public void ListenForCommand(string command, KeywordAction callback)
		{
			// Get the list of callbacks associated with this command
			List<KeywordAction> callbacks;
			if (!_commandToFunctionMap.TryGetValue(command, out callbacks))
			{
				// If no callbacks exist, create a new list
				_commandToFunctionMap[command] = callbacks = new List<KeywordAction>();

				// and start listening for this command
				{
					// Get rid of the current recognizer if it exists
					if (_recognizer != null)
					{
						_recognizer.Stop();
						_recognizer.Dispose();
					}

					// Create a new recognizer that listens for our new command (and all existing commands)
					_recognizer = new KeywordRecognizer(_commandToFunctionMap.Keys.ToArray());
					_recognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
					_recognizer.Start();
				}
			}

			// Save the callback
			callbacks.Add(callback);
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			if (_recognizer != null)
				_recognizer.Dispose();
		}

		/// <summary>
		/// When the user says a command this function is called to to let all of the listeners know.
		/// </summary>
		/// <param name="args">recognized args</param>
		private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
		{
			// If there are callbacks associated with this command, call them
			List<KeywordAction> callbacks;
			if (_commandToFunctionMap.TryGetValue(args.text, out callbacks))
			{
				callbacks.ForEach((callback) => callback(args));
			}
		}
	}

	// Signature for functions that are called after a voice command
	public delegate void KeywordAction(PhraseRecognizedEventArgs args);
}