using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Utilities {
	public class PasswordManager {
		public static void GetPass(string key) {
	        string? pass = Encryptor.GetPassword(key);
	
	        if (pass != null) {
				if (!Encryptor.PromptMasterPassword()) return;
				Display($"{key}'s password: ", ConsoleColor.Blue);
				Display($"{pass}\n", ConsoleColor.Cyan);
	        }
			else {
				Display("Key ", ConsoleColor.Red);
				Display(key, ConsoleColor.Yellow);
				Display(" does not exist.\n", ConsoleColor.Red);
			}
	    }

		public static void PromptAddPass() {
			Console.Clear();
			Console.WriteLine("Enter a key and password in the form [key] [password]:");
			
			Match match = Regex.Match(Console.ReadLine() ?? "", @"\[(.+)\] \[(.+)\]");

			if (!match.Success) {
				Display("Incorrect formatting.\n", ConsoleColor.Red);
				return;
			}

			if (!Encryptor.PromptMasterPassword()) return;

			string key = match.Groups[1].Value;
	        string password = match.Groups[2].Value;
	
	        Encryptor.Add(key, password);
			
			Display($"Added ", ConsoleColor.Blue);
			Display($"{key}", ConsoleColor.Cyan);
			Display($".\n", ConsoleColor.Blue);

			Console.ReadKey(true);
		}
		
		public static void PromptRemovePass() {
			if (!Encryptor.PromptMasterPassword()) return;

			string[] keys = Encryptor.GetAll();
			Menu menu = new Menu("Select a key:", keys);
			menu.Run();
			Console.WriteLine();
			for (int i = 0; i < keys.Length; i++) {
				if (keys[i] == keys[menu.SelectedIndex]) {
					Encryptor.Remove(keys[i]);
					Display($"Removed ", ConsoleColor.Blue);
					Display($"{keys[i]}", ConsoleColor.Cyan);
					Display($".\n", ConsoleColor.Blue);
					break;
				}
			}
			Console.ReadKey(true);
		}

		public static void PromptSelectKey() {
			if (!Encryptor.PromptMasterPassword()) return;
			Console.Clear();
			
			string[] keys = Encryptor.GetAll();
			Menu menu = new Menu("Select a key:", keys);
			menu.Run();
			Console.WriteLine();
			for (int  i = 0; i < keys.Length; i++) {
				if (keys[i] == keys[menu.SelectedIndex]) {
					Display($"{keys[i]}'s password: ", ConsoleColor.Blue);
					Display($"{Encryptor.GetPassword(keys[i])}\n", ConsoleColor.Cyan);
					 break;;
				}
			}
			Console.ReadKey(true);
		}

		public static void ShowAllPasswords() {
			if (!Encryptor.PromptMasterPassword()) return;

			Console.Clear();
			Display("All stored keys + passwords:\n", ConsoleColor.DarkBlue);
			foreach (string key in Encryptor.GetAll()) {
				if (key == ".MASTER") continue;
				string pass = Encryptor.GetPassword(key);
				Display($"{key}'s password: ", ConsoleColor.Blue);
				Display($"{pass}\n", ConsoleColor.Cyan);
			}
			Console.ReadKey(true);
		}

		private static void Display(string message, ConsoleColor col) {
			Console.ForegroundColor = col;
			Console.Write(message);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}