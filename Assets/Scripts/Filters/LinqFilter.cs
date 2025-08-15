using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CSharpAPI.Models;

namespace CSharpAPI.Filters
{
    public static class LinqFilter
    {
        public static List<string> GetAllTeams(List<Match> matches)
        {
            if (matches == null) return new List<string>();

            return matches
                .SelectMany(m => new[] { m.HomeTeam, m.AwayTeam })
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .OrderBy(name => name)
                .ToList();
        }
        
        public static List<Match> GetLastMatches(List<Match> matches, string team, int count = 5)
        {
            if (matches == null || string.IsNullOrEmpty(team))
                return new List<Match>();

            return matches
                .Where(m =>
                    m.HomeTeam.Equals(team, StringComparison.OrdinalIgnoreCase) ||
                    m.AwayTeam.Equals(team, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => ExtractRoundNumber(m.Round))
                .Take(count)
                .ToList();
        }
        private static int ExtractRoundNumber(string round)
        {
            if (string.IsNullOrEmpty(round)) return 0;
            var match = Regex.Match(round, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
