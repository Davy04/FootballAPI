using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using CSharpAPI.Models;

namespace CSharpAPI.Filters
{
    public class LinqFilter
    {
        public static void FilterAllTeams(List<Match> matches)
        {
            var allTeams = matches.Select(m => m.HomeTeam).Distinct().OrderBy(name => name);
            foreach (var team in allTeams)
            {
                Debug.Log($"- {team}");
            }
        }

        public static void FilterMatchByTeam(List<Match> matches, string team)
        {
            var filtered = matches
                .Where(match =>
                    match.HomeTeam.Contains(team, StringComparison.OrdinalIgnoreCase) ||
                    match.AwayTeam.Contains(team, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Debug.Log($"--- Games found for: {team} ---");

            foreach (var match in filtered)
            {
                match.ShowDetails();
                Debug.Log("------------------------");
            }

            if (filtered.Count == 0)
            {
                Debug.Log("No games found.");
            }
        }

        public static void FilterByResult(List<Match> matches, string team)
        {
            var winMatches = matches
                .Where(match =>
                    (match.HomeTeam.Equals(team, StringComparison.OrdinalIgnoreCase) && match.Score.Ft[0] > match.Score.Ft[1]) ||
                    (match.AwayTeam.Equals(team, StringComparison.OrdinalIgnoreCase) && match.Score.Ft[1] > match.Score.Ft[0]))
                .ToList();

            Debug.Log($"Games won by {team}");

            foreach (var match in winMatches)
            {
                match.ShowDetails();
                Debug.Log("-------------------");
            }

            if (winMatches.Count == 0)
            {
                Debug.Log($"{team} has no victories.");
            }

            Debug.Log($"Total wins: {winMatches.Count}");
        }
    }
}
