using System;
using System.Text;

namespace Utilities.CalcUtil {
	internal static class BetterInput {
		public static string Read(Action<string> onUserInput) {
			StringBuilder sb = new StringBuilder();
        	int cursorInd = 0;
			
        	Console.CursorVisible = false;
        	Console.Clear();

			string str, side1, side2;
			ConsoleKeyInfo key;
			while (true) {
				str = sb.ToString();
				side1 = str[..cursorInd];
				side2 = str[cursorInd..];
				Console.Write($"{side1}|{side2}");
				onUserInput(sb.ToString());

                key = Console.ReadKey(true);
				switch (key.Key) {
					case ConsoleKey.LeftArrow:
						if (cursorInd > 0) cursorInd--;
						break;
					case ConsoleKey.RightArrow:
						if (cursorInd < str.Length) cursorInd++;
						break;
					case ConsoleKey.Backspace:
						if (cursorInd > 0) {
							sb.Remove(cursorInd - 1, 1);
							cursorInd--;
						}
						break;
					case ConsoleKey.Escape:
						Console.Clear();
						Console.CursorVisible = true;
						return sb.ToString();
					case ConsoleKey.Enter:
						Console.Clear();
						Console.CursorVisible = true;
						return sb.ToString();
					default:
						sb.Insert(cursorInd, key.KeyChar);
						cursorInd++;
						break;
				}
				
				Console.Clear();
			}
		}
	}
}