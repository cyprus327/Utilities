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

		Console.WriteLine("Free to play games only? (Y/N)");
		string response = Console.ReadKey().KeyChar.ToString().ToLower();
		Console.WriteLine();
		
		if (response == "y") {
			sb.Append("&maxprice=free");
		}
		
		return sb.ToString();
	}
}