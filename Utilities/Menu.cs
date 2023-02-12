using System;
using System.Linq;
using System.Collections.Generic;

namespace Utilities {
	internal class Menu {
		public Menu(string title, string[] options) {
			Title = title;
			Options = options;
		}

		public string Title { get; set; }
		public string[] Options { get; set; }
		public int SelectedIndex { get; private set; } = 0;

		public void Run() {
			ConsoleKey key;
			do {
				Console.Clear();
				DisplayOptions();

				key = Console.ReadKey(true).Key;

				if (key == ConsoleKey.DownArrow) {
					SelectedIndex++;
					SelectedIndex = SelectedIndex >= Options.Length ? 0 : SelectedIndex;
					continue;
				}

				if (key == ConsoleKey.UpArrow) {
					SelectedIndex--;
					SelectedIndex = SelectedIndex < 0 ? Options.Length - 1 : SelectedIndex;
					continue;
				}

				if (key == ConsoleKey.Escape) {
					SelectedIndex = -1;
					return;
				}
			}
			while (key != ConsoleKey.Enter);
		}

		private void DisplayOptions() {
			Console.WriteLine($"{Title}\n");
			for (int i = 0; i < Options.Length; i++) {
				if (i == SelectedIndex) {
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine($"> {Options[i]}");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else {
					Console.WriteLine($"  {Options[i]}");
				}
			}
		}
	}
}