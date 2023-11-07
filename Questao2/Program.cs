using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

public class Program
{
    public static async Task Main()
    {
        string teamName1 = "Paris Saint-Germain";
        int year1 = 2013;
        int totalGoals1 = await GetTotalScoredGoals(teamName1, year1);

        Console.WriteLine("Team " + teamName1 + " scored " + totalGoals1.ToString() + " goals in " + year1);

        string teamName2 = "Chelsea";
        int year2 = 2014;
        int totalGoals2 = await GetTotalScoredGoals(teamName2, year2);

        Console.WriteLine("Team " + teamName2 + " scored " + totalGoals2.ToString() + " goals in " + year2);
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year)
    {
        using (var client = new HttpClient())
        {
            string apiUrl = "https://jsonmock.hackerrank.com/api/football_matches";
            int totalGoals = 0;
            int page = 1;

            while (true)
            {
                string requestUrl = $"{apiUrl}?team1={team}&year={year}&page={page}";
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (data == null || data.Data == null || data.Data.Count == 0)
                    {
                        break;
                    }

                    foreach (var match in data.Data)
                    {
                        int team1Goals, team2Goals;
                        if (int.TryParse(match.Team1Goals, out team1Goals) && int.TryParse(match.Team2Goals, out team2Goals))
                        {
                            totalGoals += (match.Team1 == team ? team1Goals : 0) + (match.Team2 == team ? team2Goals : 0);
                        }
                    }

                    page++;
                }
                else
                {
                    throw new Exception("Failed to retrieve data from the API.");
                }
            }

            return totalGoals;
        }
    }
}

public class ApiResponse
{
    public List<MatchData> Data { get; set; }
}

public class MatchData
{
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public string Team1Goals { get; set; }
    public string Team2Goals { get; set; }
}