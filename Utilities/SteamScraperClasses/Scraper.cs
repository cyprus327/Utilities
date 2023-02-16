using System;
using System.Text;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Utilities.SteamScraperUtil {
	internal static class Scraper {
		public static void GenerateGames(out List<string> gameNames, int startPage = 0, int depth = 5) {
			string steamURL = "https://store.steampowered.com/";
			string filter = GetSearchFilter();

			gameNames = new List<string>();
			using HttpClient client = new HttpClient();
			for (int i = 0; i < depth; i++) {
				string url = $"{steamURL}search/?page={i}{filter}&supportedlang=english&ndl=1";
				string html = GetHtml(client, url);

				string pattern = "<span class=\"title\">(.*?)</span>";
				MatchCollection matches = Regex.Matches(html, pattern);

				foreach (Match match in matches) {
					gameNames.Add(match.Groups[1].Value);
				}
			}
		}

		private static string GetHtml(HttpClient client, string url) {
			HttpResponseMessage response = client.GetAsync(url).Result;
			response.EnsureSuccessStatusCode();
			string html = response.Content.ReadAsStringAsync().Result;
			return html;
		}

		private static string GetSearchFilter() {
			StringBuilder sb = new StringBuilder();
			MenuUtil.Menu menu = new MenuUtil.Menu("Select search filters:", new string[] {
				"Free to play",
				"FPS",
				"Action",
				"Indie",
				"Singleplayer",
				"Multiplayer",
				"Casual",
				"Adventure\n",
				"Clear Filters"
			});

			void AddToURL(int index, string code) {
				string str = sb.ToString();
				if (str.Contains(code)) return;

				menu.SelectOption(index);
				if (str.Contains("&tags="))
					sb.Append($"%2C{code}");
				else
					sb.Append($"&tags={code}");
			}

			while (true) {
				menu.Run(MenuUtil.MenuOptions.None);

				if (menu.SelectedIndex == -1) break;
				switch (menu.SelectedIndex) {
					case 0:
						if (sb.ToString().Contains("&maxprice=free")) break;
						menu.SelectOption(0);
						sb.Append("&maxprice=free", 0, 14);
						break;
					case 1:
						AddToURL(1, "1663");
						break;
					case 2:
						AddToURL(2, "19");
						break;
					case 3:
						AddToURL(3, "492");
						break;
					case 4:
						AddToURL(4, "4182");
						break;
					case 5:
						AddToURL(4, "3859");
						break;
					case 6:
						AddToURL(6, "597");
						break;
					case 7:
						AddToURL(7, "21");
						break;
					case 8:
						sb.Clear();
						menu.ResetOptions();
						break;
				}
			}

			Console.WriteLine("\nFinding game...");

			return sb.ToString();
		}
	}
}