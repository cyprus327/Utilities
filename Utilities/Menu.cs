using System;
using System.Linq;
using System.Collections.Generic;

namespace Utilities {
	internal class Menu {
		public Menu(string title, string[] options) {
			_title = title;
			_options = options;
		}

		readonly string _title;
		readonly string[] _options;
		
		public int SelectedIndex { get; private set; } = 0;

		public void Run() {
			ConsoleKey key;
			do {
				Console.Clear();
				DisplayOptions();

				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.DownArrow) {
					SelectedIndex++;
					SelectedIndex = SelectedIndex >= _options.Length ? 0 : SelectedIndex;
					continue;
				}

				if (key == ConsoleKey.UpArrow) {
					SelectedIndex--;
					SelectedIndex = SelectedIndex < 0 ? _options.Length - 1 : SelectedIndex;
					continue;
				}
			}
			while (key != ConsoleKey.Enter);
		}

		private void DisplayOptions() {
			Console.WriteLine($"{_title}\n");
			for (int i = 0; i < _options.Length; i++) {
				if (i == SelectedIndex) {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"> {_options[i]}");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else {
					Console.WriteLine($"  {_options[i]}");
				}
			}
		}
	}
}