using System;
using System.Linq;
using System.Collections.Generic;

namespace Utilities.MenuUtil {
	internal class Menu {
		public Menu(string title, string[] options) {
			_title = title;
			_options = new Dictionary<int, (string, bool)>();
            for (int i = 0; i < options.Length; i++) {
				_options.Add(i, (options[i], false));
			}
		}
		
		public int SelectedIndex { get; private set; } = 0;
		
		readonly Dictionary<int, (string, bool)> _options;
		readonly string _title;

		public void Run(MenuOptions? options = null) {
			ConsoleKey key;
			do {
				Console.Clear();
				DisplayOptions(options ?? MenuOptions.LargeTitle);

				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.DownArrow) {
					SelectedIndex++;
					SelectedIndex = SelectedIndex >= _options.Count ? 0 : SelectedIndex;
					continue;
				}

				if (key == ConsoleKey.UpArrow) {
					SelectedIndex--;
					SelectedIndex = SelectedIndex < 0 ? _options.Count - 1 : SelectedIndex;
					continue;
				}

				if (key == ConsoleKey.Escape) {
					SelectedIndex = -1;
					return;
				}
			}
			while (key != ConsoleKey.Enter);
		}

		public void SelectOption(int index) {
			_options[index] = (_options[index].Item1, !_options[index].Item2);
        }

		public void ResetOptions() {
            for (int i = 0; i < _options.Count; i++) {
                if (_options[i].Item2) {
                    SelectOption(i);
                }
            }
        }

		private void DisplayOptions(MenuOptions option) {
			if (MenuOptions.LargeTitle == option) {
				Console.WriteLine(ASCIIGenerator.Generate(_title));
			}
			else {
				Console.WriteLine(_title);
			}

			Console.WriteLine();
			for (int i = 0; i < _options.Count; i++) {
				if (i == SelectedIndex) {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"> {_options[i].Item1}");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else {
					Console.ForegroundColor = _options[i].Item2 == true ? ConsoleColor.Blue : ConsoleColor.White;
					Console.WriteLine($"  {_options[i].Item1}");
				}
			}
		}
	}

	internal enum MenuOptions {
		None = 0,
		LargeTitle = 1,
	}
}