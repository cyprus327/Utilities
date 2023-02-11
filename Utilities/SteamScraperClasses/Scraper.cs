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

		bool tags = false;
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
					if (picked[1]) break;
					menu.Options[1] += " *";
					if (tags)
                        sb.Append("%2C1663");
                    else
                        sb.Append("&tags=1663");
                    tags = true;
					break;
                case 2:
                    if (picked[2]) break;
                    menu.Options[2] += " *";
                    if (tags) 
						sb.Append("%2C19");
                    else 
						sb.Append("&tags=19");
                    tags = true;
                    break;
                case 3:
                    if (picked[3]) break;
                    menu.Options[3] += " *";
                    if (tags) 
						sb.Append("%2C492");
                    else 
						sb.Append("&tags=492");
                    tags = true;
                    break;
                case 4:
                    if (picked[4]) break;
                    menu.Options[4] += " *";
                    if (tags)
                        sb.Append("%2C4182");
                    else
                        sb.Append("&tags=4182");
                    tags = true;
                    break;
                case 5:
                    if (picked[5]) break;
                    menu.Options[5] += " *";
                    if (tags)
                        sb.Append("%2C3859");
                    else
                        sb.Append("&tags=3859");
                    tags = true;
                    break;
                case 6:
                    if (picked[6]) break;
                    menu.Options[6] += " *";
                    if (tags)
                        sb.Append("%2C597");
                    else
                        sb.Append("&tags=597");
                    tags = true;
                    break;
                case 7:
                    if (picked[7]) break;
                    menu.Options[7] += " *";
                    if (tags)
                        sb.Append("%2C21");
                    else
                        sb.Append("&tags=21");
                    tags = true;
                    break;
                case 8:
                    picked = new bool[8] { false, false, false, false, false, false, false, false };
					sb.Clear();
					break;
            }
		}

        Console.WriteLine("\nFinding game...");

		return sb.ToString();
	}
}