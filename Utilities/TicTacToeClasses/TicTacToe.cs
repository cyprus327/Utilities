using System;
using System.Linq;
using System.Collections.Generic;

namespace Utilities.TTTUtil {
	internal static class TicTacToe {
		static char[] squares = { '*', '*', '*', '*', '*', '*', '*', '*', '*' };
		
		public static void PlayVsHuman() {
			bool xMove = true;
			int selectedSquare = 0;
			ConsoleKey input;
			char winner;
			while (!GameOver(out winner)) {
				do {
					Console.Clear();
					RenderBoard(selectedSquare, ConsoleColor.Green, ConsoleColor.Yellow);
					
					input = Console.ReadKey(true).Key;
					
					switch (input) {
					case ConsoleKey.LeftArrow: 
						if (selectedSquare + 1 % 3 != 0 && selectedSquare - 1 >= 0) selectedSquare--;
						break;
					case ConsoleKey.RightArrow:
						if (selectedSquare + 1 % 3 != 0 && selectedSquare + 1 <= 8) selectedSquare++;
						break;
					case ConsoleKey.UpArrow:
						if (selectedSquare > 2) selectedSquare -= 3;
						break;
					case ConsoleKey.DownArrow:
						if (selectedSquare < 6) selectedSquare += 3;
						break;
					}
				}
				while (input != ConsoleKey.Enter);
	
				if (squares[selectedSquare] != '*') continue;
	
				squares[selectedSquare] = xMove ? 'X' : 'O';
	
				xMove = !xMove;
			}

			Console.Clear();
			RenderBoard(selectedSquare, ConsoleColor.Green, ConsoleColor.Yellow);
			
			squares = new char[9] { '*', '*', '*', '*', '*', '*', '*', '*', '*' };
			
			string message = winner == ' ' ? "Tie." : $"{winner} wins!";
			Console.WriteLine($"\n{message}");
			Console.ReadKey(true);
		}

		public static void PlayVsAI(bool playingFirst = false) {
			bool xMove = !playingFirst;
			int selectedSquare = 0;
		    ConsoleKey input;
		    char winner;
		    while (!GameOver(out winner)) {
		        if (xMove != playingFirst) {
		            do {
		                Console.Clear();
		                RenderBoard(selectedSquare, ConsoleColor.Green, ConsoleColor.Yellow);
		                
		                input = Console.ReadKey(true).Key;
		                
		                switch (input) {
		                case ConsoleKey.LeftArrow: 
		                    if (selectedSquare + 1 % 3 != 0 && selectedSquare - 1 >= 0) selectedSquare--;
		                    break;
		                case ConsoleKey.RightArrow:
		                    if (selectedSquare + 1 % 3 != 0 && selectedSquare + 1 <= 8) selectedSquare++;
		                    break;
		                case ConsoleKey.UpArrow:
		                    if (selectedSquare > 2) selectedSquare -= 3;
		                    break;
		                case ConsoleKey.DownArrow:
		                    if (selectedSquare < 6) selectedSquare += 3;
		                    break;
		                }
		            }
		            while (input != ConsoleKey.Enter);
		
		            if (squares[selectedSquare] != '*') continue;
		
		            squares[selectedSquare] = xMove ? 'X' : 'O';
		        } 
				else {
		            int bestScore = xMove ? int.MinValue : int.MaxValue;
		            int move = 0;
		            for (int i = 0; i < 9; i++) {
		                if (squares[i] == '*') {
		                    squares[i] = xMove ? 'X' : 'O';
		                    int score = Minimax(0, !xMove);
		                    squares[i] = '*';
		                    if ((xMove && score > bestScore) || (!xMove && score < bestScore)) {
		                        bestScore = score;
		                        move = i;
		                    }
		                }
		            }
		            squares[move] = xMove ? 'X' : 'O';
		        }
		        xMove = !xMove;
		    }
		
		    Console.Clear();
		    RenderBoard(selectedSquare, ConsoleColor.Green, ConsoleColor.Yellow);
		
		    squares = new char[9] { '*', '*', '*', '*', '*', '*', '*', '*', '*' };
		
		    string message = winner == ' ' ? "Tie." : $"{winner} wins!";
		    Console.WriteLine($"\n{message}");
		    Console.ReadKey(true);
		}

		private static int Minimax(int depth, bool maximizingPlayer) {
			//char winner = '\0';
			if (GameOver(out char winner)) return winner == 'X' ? 10 - depth : depth - 10;
		
			if (maximizingPlayer) {
				int bestScore = int.MinValue;
				for (int i = 0; i < 9; i++) {
					if (squares[i] == '*') {
						squares[i] = 'X';
						int score = Minimax(depth + 1, false);
						bestScore = Math.Max(score, bestScore);
						squares[i] = '*';
					}
				}
				return bestScore;
			} 
			else {
				int bestScore = int.MaxValue;
				for (int i = 0; i < 9; i++) {
					if (squares[i] == '*') {
						squares[i] = 'O';
						int score = Minimax(depth + 1, true);
						bestScore = Math.Min(score, bestScore);
						squares[i] = '*';
					}
				}
				return bestScore;
			}
		}
		
		private static void RenderBoard(int selectedSquare, ConsoleColor xCol, ConsoleColor oCol, string rightOffset = "\t") {
			for (int i = 0; i < 9; i += 3) {
				Display($"{rightOffset} {squares[i]}", i == selectedSquare ? ConsoleColor.Cyan : 
					squares[i] == 'X' ? xCol : 
					squares[i] == 'O' ? oCol : ConsoleColor.White);
				
				Console.Write(" | ");
				
				Display($"{squares[i + 1]}", i + 1 == selectedSquare ? ConsoleColor.Cyan : 
					squares[i + 1] == 'X' ? xCol : 
					squares[i + 1] == 'O' ? oCol : ConsoleColor.White);
				
				Console.Write(" | ");
				
				Display($"{squares[i + 2]}\n", i + 2 == selectedSquare ? ConsoleColor.Cyan : 
					squares[i + 2] == 'X' ? xCol : 
					squares[i + 2] == 'O' ? oCol : ConsoleColor.White);
				
				if (i < 6) Console.WriteLine($"{rightOffset}-----------");
			}
		}

		private static bool GameOver(out char winner) {
			if (HasWon('X')) {
				winner = 'X';
				return true;
			}
			if (HasWon('O')) {
				winner = 'O';
				return true;
			}

			winner = ' ';
			return !squares.Contains('*');
		}

		private static bool HasWon(char player) {
			// vertical
			for (int i = 0; i < 3; i++) {
				if (squares[i] == player && squares[i + 3] == player && squares[i + 6] == player)
					return true;
			}

			// horizontal
			if (squares[0] == player && squares[1] == player && squares[2] == player)
				return true;
			if (squares[3] == player && squares[4] == player && squares[5] == player)
				return true;
			if (squares[6] == player && squares[7] == player && squares[8] == player)
				return true;

			// diagonals
			if (squares[0] == player && squares[4] == player && squares[8] == player)
				return true;
			if (squares[2] == player && squares[4] == player && squares[6] == player)
				return true;
			
			return false;
		}
		
		private static void Display(string message, ConsoleColor col) {
			Console.ForegroundColor = col;
			Console.Write(message);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}

