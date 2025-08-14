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
        
        public static string BuildResultsBlock(List<Match> matches, string team)
        {
            if (matches == null || matches.Count == 0)
                return "Nenhuma partida encontrada.";

            var sb = new StringBuilder();

            foreach (var m in matches)
            {
                sb.AppendLine(FormatResultLine(m, team, useColors: true));
            }

            return sb.ToString();
        }
        
        public static string FormatResultLine(Match m, string team, bool useColors = true)
        {
            if (m?.Score?.Ft == null || m.Score.Ft.Count < 2)
                return $"{m.HomeTeam} ? x ? {m.AwayTeam}";

            int home = m.Score.Ft[0];
            int away = m.Score.Ft[1];

            bool isHome = m.HomeTeam.Equals(team, StringComparison.OrdinalIgnoreCase);

            int goalsFor = isHome ? home : away;
            int goalsAgainst = isHome ? away : home;

            string outcome = goalsFor > goalsAgainst ? "W" :
                             goalsFor < goalsAgainst ? "L" : "D";

            string color = outcome == "W" ? "#2ECC71" : outcome == "L" ? "#E74C3C" : "#F1C40F";
            string outcomeText = useColors
                ? $"<b><color={color}>[{outcome}]</color></b>"
                : $"[{outcome}]";

            return $"{outcomeText} {m.HomeTeam} {home} x {away} {m.AwayTeam}";
        }
        
        private static int ExtractRoundNumber(string round)
        {
            if (string.IsNullOrEmpty(round)) return 0;
            var match = Regex.Match(round, @"\d+");
            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
