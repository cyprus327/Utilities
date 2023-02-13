using System;
using System.Linq;
using System.Collections.Generic;

namespace Utilities.MenuUtil {
	internal class Menu {
		public Menu(string title, string[] options) {
			Title = title;
			Options = options;
		}

		public string Title { get; set; }
		public string[] Options { get; set; }
		public int SelectedIndex { get; private set; } = 0;

		public void Run(MenuOptions? options = null) {
			ConsoleKey key;
			do {
				Console.Clear();
				DisplayOptions(options ?? MenuOptions.LargeTitle);

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

		private void DisplayOptions(MenuOptions options) {
			if (MenuOptions.LargeTitle == options) {
				Console.WriteLine(ASCIIGenerator.Generate(Title));
			}
			else {
				Console.WriteLine(Title);
			}

			Console.WriteLine();
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

	internal enum MenuOptions {
		None = 0,
		LargeTitle = 1,
	}
}