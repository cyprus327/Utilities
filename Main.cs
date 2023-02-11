using Utilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class Program {
    internal static void Main() {		
		Menu menu = new Menu("Utilities.", new string[] {
			"Password Manager", 
			"Calculator", 
			"Chess",
			"Tic Tac Toe",
			"Get Random Steam Game\n",
			"Exit" 
		});

		while (true) {
			menu.Run();
		
			switch (menu.SelectedIndex) {
				case 0: RunPasswordManager(); break;
				case 1: RunCalculator(); break;
				case 2: RunChess(); break;
				case 3: RunTicTacToe(); break;
				case 4: RunSteamScraper(); break;
				case 5: Environment.Exit(0); break;
				default: break;
			}
		}
    }

    static void RunPasswordManager() {
		Console.Clear();
		Console.WriteLine("Encrypting passwords...");
		Console.WriteLine("Initalizing...\n");
        Encryptor.Init();
        Encryptor.Add("ebay", "EbayPass123");
		Encryptor.Add("amazon", "123amazon321");
		Encryptor.Add("gmail", "800G73");
		Encryptor.Add("important", "password01");
		
        Console.Clear();
        Menu menu = new Menu("Password Manager.", new string[] {
			"Select",
			"Add",
			"Remove",
			"Show all passwords"
		});

		while (true) {
			menu.Run();
	
			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: PasswordManager.PromptSelectKey(); break;
				case 1: PasswordManager.PromptAddPass(); break;
				case 2: PasswordManager.PromptRemovePass(); break;
				case 3: PasswordManager.ShowAllPasswords(); break;
			}
		}
    }

	static void RunCalculator() {
		Console.Clear();
		Console.WriteLine("Enter an expression or 'exit' to exit:");

		while (true) {
			string expression = BetterInput.Read();
			if (expression == "exit" || expression == "") return;
			
			if (expression.Contains("=")) {
				Display("Enter one side only, e.g. 9.5 / (1 - 4)\n", ConsoleColor.Red);
				RunCalculator();
			}
	
			double result = Evaluator.EvaluateExpression(expression);
			Display(expression, ConsoleColor.White);
			Display(" = ", ConsoleColor.Cyan);
			Display($"{result.ToString()}\n", ConsoleColor.Blue);
			
			var key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Escape) return;
		}
	}

	private static void RunChess() {
		Menu menu = new Menu("Chess.", new string[] {
			"AI opponent",
			"Human opponent"
		});

		while (true) {
			menu.Run();

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: Chess.PlayVsAI(); break;
				case 1: Chess.PlayVsHuman(); break;
			}
		}
	}

	private static void RunTicTacToe() {
		Menu menu = new Menu("Tic Tac Toe.", new string[] {
			"AI opponent",
			"Human opponent"
		});

		while (true) {
			menu.Run();

			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: TicTacToe.PlayVsAI(); break;
				case 1: TicTacToe.PlayVsHuman(); break;
			}
		}
	}

	private static void RunSteamScraper() {
		Menu menu = new Menu("Select amount of games to search.", new string[] {
			"Low",
			"Medium",
			"High",
		});

		List<string> gameNames;
		int rand;
		string game;

		while (true) {
			menu.Run();
			Console.WriteLine();
			switch (menu.SelectedIndex) {
				case -1: return;
				case 0: 
					Scraper.GenerateGames(out gameNames, 0, 5);
					rand = RandomNumberGenerator.GetInt32(gameNames.Count - 1);
					game = gameNames[rand];
					Display("\nYou should play: ", ConsoleColor.Blue);
					Display(game + "\n", ConsoleColor.Cyan);
					Console.ReadKey(true);
					break;
				case 1:
					Scraper.GenerateGames(out gameNames, 0, 12);
					rand = RandomNumberGenerator.GetInt32(gameNames.Count - 1);
					game = gameNames[rand];
					Display("\nYou should play: ", ConsoleColor.Blue);
					Display(game + "\n", ConsoleColor.Cyan);
					Console.ReadKey(true);
					break;
				case 2:
					Scraper.GenerateGames(out gameNames, 0, 30);
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