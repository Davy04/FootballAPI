using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpAPI.Models
{
    public class BestMatches
    {
        public string Name { get; set; }
        public List<Match> bestMatchesList { get; }

        public BestMatches(string name)
        {
            Name = name;
            bestMatchesList = new List<Match>();
        }

        public void AddBestMatch(Match match)
        {
            bestMatchesList.Add(match);
        }

        public void ShowBestsMatches()
        {
            Debug.Log($"Best Matches by: {Name}");
            foreach (var match in bestMatchesList)
            {
                Debug.Log($"-{match.HomeTeam}: {match.Score.Ft[0]} x {match.Score.Ft[1]} :{match.AwayTeam}-");
            }
        }
    }
}