using Utilities;
using System;

internal class Program {
    internal static void Main() {			
		Menu menu = new Menu("Utilities.", new string[] {
			"Password Manager", 
			"Calculator", 
			"Chess",
			"Tic Tac Toe\n",
			"Exit" 
		});

		while (true) {
			menu.Run();
		
			switch (menu.SelectedIndex) {
				case 0: RunPasswordManager(); break;
				case 1: RunCalculator(); break;
				case 2: RunChess(); break;
				case 3: RunTicTacToe(); break;
				case 4: Environment.Exit(0); break;
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
			"Show all passwords",
			"Back"
		});

		while (true) {
			menu.Run();
	
			switch (menu.SelectedIndex) {
				case 0: PasswordManager.PromptSelectKey(); break;
				case 1: PasswordManager.PromptAddPass(); break;
				case 2: PasswordManager.PromptRemovePass(); break;
				case 3: PasswordManager.ShowAllPasswords(); break;
				case 4: return;
			}
		}
    }

	static void RunCalculator() {
		Console.Clear();
		Console.WriteLine("Enter an expression or 'exit' to exit:");

		while (true) {
			string expression = BetterInput.Read();
			if (expression == "exit") return;
			
			if (expression == "!") {
				Display("Invalid expression.\n", ConsoleColor.Red);
				RunCalculator();
			}
			else if (expression.Contains("=")) {
				Display("Enter one side only, e.g. 9.5 / (1 - 4)\n", ConsoleColor.Red);
				RunCalculator();
			}
	
			double result = Evaluator.EvaluateExpression(expression);
			Display(expression, ConsoleColor.White);
			Display(" = ", ConsoleColor.Cyan);
			Display($"{result.ToString()}\n", ConsoleColor.Blue);
			Console.ReadKey(true);
		}
	}

	private static void RunChess() {
		Menu menu = new Menu("Chess.", new string[] {
			"AI opponent",
			"Human opponent",
			"Show all moves",
			"Back"
		});

		while (true) {
			menu.Run();

			switch (menu.SelectedIndex) {
				case 0: Chess.PlayVsAI(); break;
				case 1: Chess.PlayVsHuman(); break;
				case 2: Chess.StepThroughMoves(); break;
				case 3: return;
			}
		}
	}

	private static void RunTicTacToe() {
		Menu menu = new Menu("Tic Tac Toe.", new string[] {
			"AI opponent",
			"Human opponent",
			"Back"
		});

		while (true) {
			menu.Run();

			switch (menu.SelectedIndex) {
				case 0: TicTacToe.PlayVsAI(); break;
				case 1: TicTacToe.PlayVsHuman(); break;
				case 2: return;
			}
		}
	}

	private static void Display(string message, ConsoleColor col) {
		Console.ForegroundColor = col;
		Console.Write(message);
		Console.ForegroundColor = ConsoleColor.White;
	}
}