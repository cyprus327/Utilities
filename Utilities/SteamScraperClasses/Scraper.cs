using System;
using System.Text;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
		bool tags = false;
		bool[] picked = { false, false, false, false, false, false, false, false };
		Utilities.Menu menu = new Utilities.Menu("Select search filters:", new string[] {
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

        void Add(int index, string code) {
            if (picked[index]) return;
            
            menu.Options[index] += " *";

            if (tags)
                sb.Append($"%2C{code}");
            else
                sb.Append($"&tags={code}");

            if (!tags) tags = true;

        }

		while (true) {
			menu.Run();

			if (menu.SelectedIndex == -1) break;
			switch (menu.SelectedIndex) {
				case 0:
					if (picked[0]) break;
                    menu.Options[0] += " *";
                    sb.Append("&maxprice=free", 0, 14);
					break;
				case 1:
                    Add(1, "1663");
					break;
                case 2:
                    Add(2, "19");
                    break;
                case 3:
                    Add(3, "492");
                    break;
                case 4:
                    Add(4, "4182");
                    break;
                case 5:
                    Add(5, "3859");
                    break;
                case 6:
                    Add(6, "597");
                    if (picked[6]) break;
                    break;
                case 7:
					Add(7, "21");
                    if (picked[7]) break;
                    break;
                case 8:
                    picked = new bool[8] { false, false, false, false, false, false, false, false };
					sb.Clear();
                    tags = false;
					break;
            }
		}

        Console.WriteLine("\nFinding game...");

		return sb.ToString();
	}
}