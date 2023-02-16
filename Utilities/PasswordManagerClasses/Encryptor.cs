using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.IO;

namespace Utilities.PMUtil {
    internal static class Encryptor {
        static string SEC = "qwertyuiopasdfghjklzxcvbnm,./;=-!@#$%^&*()1234567890\\~`\n\n\n\t\t\t";
        const string PRED = "Manager";
		const char S = '\\';

        public static void Init() {
			if (Directory.Exists(PRED)) {
				SEC = Encoding.UTF8.GetString(File.ReadAllBytes($"{PRED}{S}zzzz.zzzz"));
				if (Directory.Exists($"{PRED}{S}.MASTER")) return;
			}
			
			Directory.CreateDirectory(PRED);
			SEC = new string(SEC.OrderBy(c => Guid.NewGuid()).ToArray());
			File.WriteAllBytes($"{PRED}{S}zzzz.zzzz", Encoding.UTF8.GetBytes(SEC));

			Console.WriteLine("Enter a master password:");
			string password = Console.ReadLine() ?? "";
			if (password.Length < 14) { // change to much longer
				Console.WriteLine("Password length must be greater than 14.");
				Init();
				return;
			}
			
			Add(".MASTER", password);
			Console.WriteLine("Master password saved, press any key to continue.");
			Console.ReadKey(true);
        }
		
		public static string[] GetAll() {			
            List<string> output = new List<string>();
            foreach (string filename in Directory.GetDirectories(PRED)) {
				string key = filename.Split(S)[1];
				if (key == ".MASTER") continue;
                output.Add(key);
            }
            return output.ToArray();
        }

        public static string? GetPassword(string key) {
			if (!KeyExists(key)) return null;
			
            byte[] decrypted = Retrieve(key);
            return Encoding.UTF8.GetString(decrypted);
        }

        public static void Add(string key, string password) {
			byte[] encryptedPassword = Encrypt(Encoding.UTF8.GetBytes(password), out byte[] salt);
			
            Directory.CreateDirectory($"{PRED}{S}{key}");
            File.WriteAllBytes($"{PRED}{S}{key}{S}{key}.pass", encryptedPassword);
            File.WriteAllBytes($"{PRED}{S}{key}{S}{key}.salt", salt);
        }

		public static void Remove(string key) {
			File.Delete($"{PRED}{S}{key}{S}{key}.pass");
			File.Delete($"{PRED}{S}{key}{S}{key}.salt");
			Directory.Delete($"{PRED}{S}{key}");
		}
		
		public static bool KeyExists(string key) {
			return Directory.Exists($"{PRED}{S}{key}");
		}
		
		public static bool PromptMasterPassword() {
			Console.WriteLine("\nEnter master passsword:");

			byte[] master = Retrieve(".MASTER");
			byte[] response = Encoding.UTF8.GetBytes(Console.ReadLine() ?? "");
			if (Compare(master, response)) {
				return true;
			}

			Console.WriteLine("Incorrect master password.");
			return false;
		}
		
        public static byte[] GenerateSalt() {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[32];
            rng.GetBytes(salt);
            rng.Dispose();
            return salt;
        }

        public static bool Compare(byte[] a, byte[] b) {
            if (a.Length != b.Length) return false;

            int difference = 0;
            for (int i = 0; i < a.Length; i++) {
                difference |= a[i] ^ b[i];
			}
			return difference == 0;
        }

        public static byte[] Encrypt(byte[] plain, out byte[] salt) {
			salt = GenerateSalt();
			using var rfcDeriv = new Rfc2898DeriveBytes(SEC, salt, 10000);
            using Aes aes = Aes.Create();
            aes.Key = rfcDeriv.GetBytes(32);
            aes.IV = rfcDeriv.GetBytes(16);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(plain, 0, plain.Length);
        }

        public static byte[] Decrypt(byte[] cipher, byte[] key, byte[] iv) {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = aes.CreateDecryptor();
            try {
                return decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            }
            catch (CryptographicException) {
                return new byte[byte.MaxValue];
            }
        }

        private static byte[] Retrieve(string key) {
            byte[] storedEncryptedPassword = File.ReadAllBytes($"{PRED}{S}{key}{S}{key}.pass");
            byte[] fileSalt = File.ReadAllBytes($"{PRED}{S}{key}{S}{key}.salt");

            using var derivation = new Rfc2898DeriveBytes(SEC, fileSalt, 10000);
            byte[] enteredKey = derivation.GetBytes(32);
            byte[] enteredIv = derivation.GetBytes(16);

            return Decrypt(storedEncryptedPassword, enteredKey, enteredIv);
        }
    }
}