using Utilities.MenuUtil;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class Program {
    internal static void Main() {		
		Menu menu = new Menu("Utilities.", new string[] {
			"Password Manager", 
			"Calculator", 
			"Games",
			"Get Random Steam Game\n",
			"Exit" 
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);
		
			switch (menu.SelectedIndex) {
				case 0: RunPasswordManager(); break;
				case 1: RunCalculator(); break;
				case 2: RunGamesMenu(); break;
				case 3: RunSteamScraper(); break;
				case 4: Environment.Exit(0); break;
				default: break;
			}
		}
    }

    private static void RunPasswordManager() {
		Console.Clear();
		Console.WriteLine("Encrypting passwords...");
		Console.WriteLine("Initalizing...\n");
        Utilities.PMUtil.Encryptor.Init();
        Utilities.PMUtil.Encryptor.Add("ebay", "EbayPass123");
        Utilities.PMUtil.Encryptor.Add("amazon", "123amazon321");
        Utilities.PMUtil.Encryptor.Add("gmail", "800G73");
        Utilities.PMUtil.Encryptor.Add("important", "password01");
		
        Console.Clear();
        Menu menu = new Menu("Passwords.", new string[] {
			"Select",
			"Add",
			"Remove",
			"Show all passwords"
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);
	
			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: Utilities.PMUtil.PasswordManager.PromptSelectKey(); break;
				case 1: Utilities.PMUtil.PasswordManager.PromptAddPass(); break;
				case 2: Utilities.PMUtil.PasswordManager.PromptRemovePass(); break;
				case 3: Utilities.PMUtil.PasswordManager.ShowAllPasswords(); break;
			}
		}
    }

	private static void RunCalculator() {
		Console.Clear();
		Console.WriteLine("Enter an expression or 'exit' to exit:");

		while (true) {
			string expression = Utilities.CalcUtil.BetterInput.Read(s => {
				Display(" = ", ConsoleColor.Cyan);
				Display($"{Utilities.CalcUtil.Evaluator.EvaluateExpression(s)}\n", ConsoleColor.Blue);
			});
			if (expression == "exit" || expression == "") return;

			double result = Utilities.CalcUtil.Evaluator.EvaluateExpression(expression);
			Display(expression, ConsoleColor.White);
			Display(" = ", ConsoleColor.Cyan);
			Display($"{result}\n", ConsoleColor.Blue);
			
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Escape) return;
		}
	}

	private static void RunGamesMenu() {
		Menu menu = new Menu("Games.", new string[] {
			"Chess",
			"Tic Tac Toe",
			"Pong"
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: RunChess(); break;
				case 1: RunTicTacToe(); break;
				case 2: RunPong(); break;
			}
		}
	}

	private static void RunChess() {
		Menu menu = new Menu("Chess.", new string[] {
			"AI opponent",
			"Human opponent"
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: Utilities.ChessUtil.Chess.PlayVsAI(); break;
				case 1: Utilities.ChessUtil.Chess.PlayVsHuman(); break;
			}
		}
	}

	private static void RunTicTacToe() {
		Menu menu = new Menu("TicTacToe.", new string[] {
			"AI opponent",
			"Human opponent"
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: Utilities.TTTUtil.TicTacToe.PlayVsAI(); break;
				case 1: Utilities.TTTUtil.TicTacToe.PlayVsHuman(); break;
			}
		}
	}

	private static void RunPong() {
		Menu menu = new Menu("Pong.", new string[] {
			"AI opponent",
			"Human opponent"
		});

		while (true) {
			menu.Run(MenuOptions.LargeTitle);

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: Utilities.PongUtil.Pong.PlayVsAI(); break;
				case 1: Utilities.PongUtil.Pong.PlayVsHuman(); break;
			}
		}
	}

	private static void RunSteamScraper() {
		Menu menu = new Menu("Steam.", new string[] {
			"Low",
			"Medium",
			"High",
		});

		List<string> gameNames;
		int rand;
		string game;

		while (true) {
			menu.Run(MenuOptions.LargeTitle);
			Console.WriteLine();
			switch (menu.SelectedIndex) {
				case -1: return;
				case 0:
                    Utilities.SteamScraperUtil.Scraper.GenerateGames(out gameNames, 0, 5);
					rand = RandomNumberGenerator.GetInt32(gameNames.Count - 1);
					game = gameNames[rand];
					Display("\nYou should play: ", ConsoleColor.Blue);
					Display(game + "\n", ConsoleColor.Cyan);
					Console.ReadKey(true);
					break;
				case 1:
                    Utilities.SteamScraperUtil.Scraper.GenerateGames(out gameNames, 0, 12);
					rand = RandomNumberGenerator.GetInt32(gameNames.Count - 1);
					game = gameNames[rand];
					Display("\nYou should play: ", ConsoleColor.Blue);
					Display(game + "\n", ConsoleColor.Cyan);
					Console.ReadKey(true);
					break;
				case 2:
                    Utilities.SteamScraperUtil.Scraper.GenerateGames(out gameNames, 0, 30);
					rand = RandomNumberGenerator.GetInt32(gameNames.Count - 1);
					game = gameNames[rand];
					Display("\nYou should play: ", ConsoleColor.Blue);
					Display(game + "\n", ConsoleColor.Cyan);
					Console.ReadKey(true);
					break;
			}
		}
	}

	private static void Display(string message, ConsoleColor col) {
		Console.ForegroundColor = col;
		Console.Write(message);
		Console.ForegroundColor = ConsoleColor.White;
	}
}